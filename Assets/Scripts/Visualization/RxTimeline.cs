using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RxTimeline : MonoBehaviour
{
    public Image progressLine;
    public RxMessageComponent nextPrefab;
    public GameObject initializedPrefab;
    public GameObject completedPrefab;
    public GameObject errorPrefab;
    public RectTransform markerContainer;
    public RectTransform overlayMarkerContainer;
    public TextMeshProUGUI timelineTitle;

    private bool _updateProgress;
    private float _startProgress;

    public string Title { get => timelineTitle.text; set => timelineTitle.text = value; }

    public void Initialize(float startProgress)
    {
        _startProgress = startProgress;
    }

    public void CreateMarker(float progress, RxMessageType messageType, int? value = 0, Color? color = null)
    {
        switch (messageType)
        {
            case RxMessageType.Next:
                var messageMarker = Instantiate(nextPrefab, markerContainer);
                messageMarker.Color = color ?? Color.white;
                messageMarker.Message = value?.ToString();
                PlaceMarker(progress, messageMarker.GetComponent<RectTransform>());
                break;
            case RxMessageType.Completed:
                PlaceMarker(progress, Instantiate(completedPrefab, overlayMarkerContainer).GetComponent<RectTransform>());
                SetProgress(progress);
                _updateProgress = false;
                break;
            case RxMessageType.Error:
                PlaceMarker(progress, Instantiate(errorPrefab, overlayMarkerContainer).GetComponent<RectTransform>());
                SetProgress(progress);
                _updateProgress = false;
                break;
        }
    }

    public void PlaceMarker(float progress, RectTransform markerTransform)
    {
        var width = markerTransform.parent.GetComponent<RectTransform>().rect.width;
        var xPosition = progress * width;
        markerTransform.anchorMin = new Vector2(0, 0.5f);
        markerTransform.anchorMax = new Vector2(0, 0.5f);
        markerTransform.anchoredPosition = new Vector2(xPosition, 0);
    }

    private void Update()
    {
        if (_updateProgress)
        {
            SetProgress(RxTimelineManager.Main.GetProgress(Time.time));
        }
    }

    private void Start()
    {
        _updateProgress = true;
        Observable.NextFrame().Subscribe(_ =>
        {
            var progressLineTransform = progressLine.GetComponent<RectTransform>();
            var positionDelta = Vector2.right * _startProgress;
            progressLineTransform.anchorMin += positionDelta;
            progressLineTransform.anchorMax += positionDelta;
            var marker = Instantiate(initializedPrefab, overlayMarkerContainer).GetComponent<RectTransform>();
            PlaceMarker(_startProgress, marker);
        }).AddTo(this);
    }

    private void SetProgress(float progress)
    {
        progressLine.fillAmount = progress - _startProgress;
    }
}
