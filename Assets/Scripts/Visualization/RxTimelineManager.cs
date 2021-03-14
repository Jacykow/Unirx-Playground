using System;
using UniRx;
using UnityEngine;

public class RxTimelineManager : MonoBehaviour
{
    public RxTimeline timelinePrefab;

    public static RxTimelineManager Main { get; private set; }
    public const float TimelineDuration = 10f;

    public float TimeStart { get; private set; }
    public float TimeEnd { get; private set; }

    public float GetProgress(float time)
    {
        return Mathf.InverseLerp(TimeStart, TimeEnd, time);
    }

    public CompositeDisposable DisposeBag { get; private set; }

    public IObservable<RxMessageVisualizationData> VisualizeObservable(IObservable<RxMessageVisualizationData> observable, string title = null)
    {
        var timeline = Instantiate(timelinePrefab, transform);
        timeline.Title = title;
        timeline.Initialize(GetProgress(Time.time));
        return observable
            .Do(value => timeline.CreateMarker(GetProgress(Time.time), RxMessageType.Next, value.Value, value.Color))
            .DoOnCompleted(() => timeline.CreateMarker(GetProgress(Time.time), RxMessageType.Completed))
            .DoOnError(error => timeline.CreateMarker(GetProgress(Time.time), RxMessageType.Error));
    }

    private void Awake()
    {
        Main = this;
        TimeStart = Time.time;
        TimeEnd = TimeStart + TimelineDuration;
        DisposeBag = new CompositeDisposable();
    }

    private void Start()
    {
        Observable.EveryUpdate().AsUnitObservable().Where(_ => Time.time >= TimeEnd).Take(1).Subscribe(_ =>
        {
            DisposeBag.Dispose();
        }).AddTo(DisposeBag);
    }
}
