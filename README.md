# Unirx Playground
A Unity playground for [UniRx](https://github.com/neuecc/UniRx "UniRx Repository") visualization inspired by [rxmarbles.com](https://rxmarbles.com/ "RxMarbles").

### Usage
After downloading the source code simply press play on the [MainScene](../main/Assets/MainScene.unity). The default choice is a SelectMany visualization which is one of the prepared examples. Check out other prepared examples in the [Example](../main/Assets/Scripts/Examples/Example.cs) script. You can write your own examples in this class and select them by changing _exampleId_ in the inspector.

![Select Many Example Visualization](https://github.com/Jacykow/Unirx-Playground/blob/main/image.jpg?raw=true "Select Many")

### How does this work?
Observable usage normally would look something like this:
```csharp
SomeObservable
    .UniRxExtensionMethod1()
    .UniRxExtensionMethod2()
    .Subscribe(value => OnNext(value))
    .AddTo(this);
```
To visualize an observable you need to convert it to ```IObservable<RxMessageVisualizationData>```. Any observable of this type can be visualized by the ```Visualize``` extension method.
```csharp
SomeObservable
    .UniRxExtensionMethod1()
    .UniRxExtensionMethod2()
    .AsVisualizationDataObservable()
    .Visualize("Timeline Title")
    .Subscribe()
    .AddTo(RxTimelineManager.Main.DisposeBag);
```
The conversion to ```IObservable<RxMessageVisualizationData>``` can be done easily via the ```AsVisualizationDataObservable``` extension method. Without any given parameters the color will be a random color chosen at the beginning and the shown number will be an increasing integer starting from 1. If you want to keep any of the information that was contained in the original message you need to provide a selector method that will extract it.

All subscriptions have to be destroyed at the end of the visualizations in order to stop displying incoming messages. This can be ensured by adding them to ```RxTimelineManager.Main.DisposeBag```.

All of this is necessary for the [RxTimelineManager](../main/Assets/Scripts/Visualization/RxTimelineManager.cs) to invoke the ```Do```, ```DoOnCompleted``` and ```DoOnError``` methods.
