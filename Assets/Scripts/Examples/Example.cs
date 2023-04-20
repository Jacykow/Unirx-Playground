using System;
using UniRx;
using UnityEngine;

public class Example : MonoBehaviour
{
    [SerializeField] private ExampleType example;

    private void Start()
    {
        VisualizeEmptyTimeline();

        switch (example)
        {
            case ExampleType.Interval:
                ExampleInterval();
                break;
            case ExampleType.Merge:
                ExampleMerge();
                break;
            case ExampleType.SelectMany:
                ExampleSelectMany();
                break;
            case ExampleType.SelectManyWithException:
                ExampleSelectManyWithException();
                break;
            default:
                VisualizeEmptyTimeline();
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
            .Take(4)
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

    private void ExampleSelectManyWithException()
    {
        var baseTimer = Observable.Interval(TimeSpan.FromSeconds(1))
            .Take(4)
            .AsVisualizationDataObservable()
            .Visualize("1 second interval");
        baseTimer.SelectMany(visualizationData =>
        {
            if (visualizationData.Value == 3) throw new Exception();
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

    private enum ExampleType
    {
        Interval,
        Merge,
        SelectMany,
        SelectManyWithException
    }
}
