using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RxMessageComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private Image _background;

    public string Message { get => _text.text; set => _text.text = value; }
    public Color Color { get => _background.color; set => _background.color = value; }
}
