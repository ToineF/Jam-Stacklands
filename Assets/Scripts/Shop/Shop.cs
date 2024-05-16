using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (newDraggable == null) return;

        newDraggable.IsOverShop = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (newDraggable == null) return;

        newDraggable.IsOverShop = false;
    }
}
