using System;
using UniRx;
using UnityEngine;

public class Example : MonoBehaviour
{
    public int exampleId;

    private void Start()
    {
        VisualizeEmptyTimeline();

        switch (exampleId)
        {
            case 0:
                ExampleInterval();
                break;
            case 1:
                ExampleMerge();
                break;
            case 2:
                ExampleSelectMany();
                break;
        }
    }

    private void ExampleInterval()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .AsVisualizationDataObservable()
            .Visualize("1 second interval").Subscribe().AddTo(RxTimelineManager.Main.DisposeBag);
    }

    private void ExampleMerge()
    {
        var yellowTimer = Observable.Interval(TimeSpan.FromSeconds(1.5))
            .AsVisualizationDataObservable()
            .Visualize("1.5 second interval");
        var redTimer = Observable.Interval(TimeSpan.FromSeconds(0.8))
            .AsVisualizationDataObservable()
            .Visualize("0.8 second interval");
        Observable.Merge(yellowTimer, redTimer)
            .Visualize("Merge").Subscribe().AddTo(RxTimelineManager.Main.DisposeBag);
    }

    private void ExampleSelectMany()
    {
        var baseTimer = Observable.Interval(TimeSpan.FromSeconds(1))
            .Take(3)
            .AsVisualizationDataObservable()
            .Visualize("1 second interval");
        baseTimer.SelectMany(visualizationData =>
        {
            return Observable.Interval(TimeSpan.FromSeconds(0.8))
                .Take(3)
                .AsVisualizationDataObservable()
                .Visualize("0.8 second interval " + visualizationData.Value);
        }).Visualize("Select Many").Subscribe().AddTo(RxTimelineManager.Main.DisposeBag);
    }

    private void VisualizeEmptyTimeline()
    {
        Observable.Never<Unit>()
            .AsVisualizationDataObservable()
            .Visualize("Empty timeline").Subscribe().AddTo(RxTimelineManager.Main.DisposeBag);
    }
}
