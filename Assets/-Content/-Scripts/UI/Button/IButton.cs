using UnityEngine.EventSystems;

public interface IButton : IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData data);
    public void OnPointerUp(PointerEventData data);
    public void OnPointerEnter(PointerEventData eventData);
    public void OnPointerExit(PointerEventData eventData);
}