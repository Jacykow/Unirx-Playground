using UnityEngine;

public class RxMessageVisualizationData
{
    public int? Value { get; }
    public Color? Color { get; }

    public RxMessageVisualizationData(int? value, Color? color)
    {
        Value = value;
        Color = color;
    }
}
