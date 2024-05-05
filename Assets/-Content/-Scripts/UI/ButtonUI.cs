using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	[Header("Tweakable values")]
	[Header("Audio settings")]
	[SerializeField] private AudioClip _onClickClip;

    [Header("Scale settings")] 
    [SerializeField] private float _scaleDuration = 0.1f;
    [SerializeField] private float _scaleDownMultiplier = 0.8f;
    [SerializeField] private float _scaleUpMultiplier = 1.2f;

    [Header("Internal references")] 
    [SerializeField] private GameObject _goParent;
    [SerializeField] private Image _blackImage = null;

	[Header("External references")]
	[SerializeField] private RSE_Sound _rsePlaySound;

	[Space(10)]
	public UnityEvent OnClick;

    public void OnPointerDown(PointerEventData data)
    {
		_goParent.transform.DOScale(_scaleDownMultiplier, _scaleDuration).SetEase(Ease.OutBack);
        if (_blackImage != null) _blackImage.DOFade(0.75f, 0);
		_rsePlaySound.Call(TypeSound.SFX, _onClickClip, false);
    }

    public void OnPointerUp(PointerEventData data)
    {
       _goParent.transform.DOScale(Vector3.one, _scaleDuration).SetEase(Ease.OutBack);
        if (_blackImage != null) _blackImage.DOFade(0, 0);
        OnClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _goParent.transform.DOScale(_scaleUpMultiplier, _scaleDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _goParent.transform.DOScale(Vector3.one, _scaleDuration).SetEase(Ease.OutBack);
    }
}