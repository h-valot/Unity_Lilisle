using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnfoldOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")] 
    [SerializeField] private RectTransform _rtParent;

    [Header("Tweakable values")] 
    [SerializeField] private float _moveDuration = 0.1f;
    [SerializeField] private float _deltaSizePercentageUnfolded = 0.65f;

    public void OnPointerEnter(PointerEventData eventData) => Unfold();

    public void OnPointerExit(PointerEventData eventData) => Fold();

	public void Unfold() => _rtParent.DOLocalMoveY(_rtParent.sizeDelta.y * _deltaSizePercentageUnfolded, _moveDuration).SetEase(Ease.OutBack);

	public void Fold() => _rtParent.DOLocalMoveY(0, _moveDuration).SetEase(Ease.OutBack);
}