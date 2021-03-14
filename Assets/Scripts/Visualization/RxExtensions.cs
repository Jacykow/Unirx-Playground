using System;
using UniRx;
using UnityEngine;

public static class RxExtensions
{
    public static IObservable<T> LimitDuration<T>(this IObservable<T> observable, float duration)
    {
        return observable.TakeUntil(Observable.Timer(TimeSpan.FromSeconds(duration)));
    }

    public static IObservable<RxMessageVisualizationData> Visualize(this IObservable<RxMessageVisualizationData> observable, string title = null)
    {
        return RxTimelineManager.Main.VisualizeObservable(observable, title);
    }

    public static IObservable<RxMessageVisualizationData> AsVisualizationDataObservable<T>(this IObservable<T> observable,
         Func<T, int?> selector = null)
    {
        return observable.AsVisualizationDataObservable(null, selector);
    }

    public static IObservable<RxMessageVisualizationData> AsVisualizationDataObservable<T>(this IObservable<T> observable,
         Color? color, Func<T, int?> selector = null)
    {
        if (selector == null)
        {
            int value = 1;
            selector = _ => value++;
        }
        if (color == null)
        {
            color = Color.HSVToRGB(UnityEngine.Random.value, 1f, 1f);
        }
        return observable.AsVisualizationDataObservable(value =>
            new RxMessageVisualizationData(selector.Invoke(value), color));
    }

    public static IObservable<RxMessageVisualizationData> AsVisualizationDataObservable<T>(this IObservable<T> observable,
         Func<T, RxMessageVisualizationData> selector)
    {
        return observable.Select(value => selector.Invoke(value));
    }
}
