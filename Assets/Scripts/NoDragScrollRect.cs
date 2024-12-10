using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoDragScrollRect : ScrollRect
{
    // Prevent dragging by overriding OnBeginDrag, OnDrag, and OnEndDrag
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
}
