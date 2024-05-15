using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public Draggable CurrentDraggable { get => _currentDraggable; private set => _currentDraggable = value; }
    public Draggable SavedDraggable { get => _savedDraggable; private set => _savedDraggable = value; }

    private Draggable _currentDraggable;
    private Draggable _savedDraggable;

    public void OnDrop(PointerEventData eventData)
    {
        UpdateCurrentDraggable();

        GameObject droppedObject = eventData.pointerDrag;
        Draggable newDraggable = droppedObject.GetComponent<Draggable>();

        if (CurrentDraggable != null)
        {
            SwapDraggables(CurrentDraggable, newDraggable);
        }
        else
        {
            SetNewDraggable(newDraggable);
        }
    }

    public void UpdateCurrentDraggable()
    {
        if (CurrentDraggable != null && CurrentDraggable.ParentTransform != transform && CurrentDraggable.ParentTransform != transform.root)
        {
            CurrentDraggable = null;
        }
    }

    public void SetNewDraggable(Draggable newDraggable)
    {
        CurrentDraggable = newDraggable;
        if (CurrentDraggable != null)
            CurrentDraggable.ParentTransform = transform;
    }

    private void SwapDraggables(Draggable draggable1, Draggable draggable2)
    {
        Transform draggable1Parent = draggable1.ParentTransform;
        draggable1.ParentTransform = draggable2.ParentTransform;
        draggable2.ParentTransform = draggable1Parent;

        draggable1.transform.SetParent(draggable1.ParentTransform);
        draggable2.transform.SetParent(draggable2.ParentTransform);

        draggable1.ResetLocalPosition();
        draggable2.ResetLocalPosition();

        draggable1.ParentTransform.GetComponent<DropZone>().CurrentDraggable = draggable1;
        draggable2.ParentTransform.GetComponent<DropZone>().CurrentDraggable = draggable2;
    }

    public void SaveDraggable()
    {
        SavedDraggable = CurrentDraggable;
        UpdateCurrentDraggable();
    }

    //public void SaveAndMove(Inventory inventory) // bug when the draggable is at another inventory place when opening/closing
    //{
    //    UpdateCurrentDraggable();

    //    if (CurrentDraggable != null && SavedDraggable != null)
    //    {
    //        DropZone freeDropZone = inventory.GetFirstEmptyInventoryItem();
    //        Debug.Log(freeDropZone.name);
    //        freeDropZone.SetNewDraggable(SavedDraggable);
    //        SavedDraggable = null;
    //        Debug.Log('s');
    //        return;
    //    }

    //    Draggable tempDraggable = SavedDraggable;
    //    SavedDraggable = CurrentDraggable;

    //    if (tempDraggable == null) return;

    //    SetNewDraggable(tempDraggable);
    //    tempDraggable.transform.SetParent(tempDraggable.ParentTransform);
    //    tempDraggable.ResetLocalPosition();
    //}
}