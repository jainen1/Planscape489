using UnityEngine;
using UnityEngine.EventSystems;

public class NoDragScrollRect : UnityEngine.UI.ScrollRect
{
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
}