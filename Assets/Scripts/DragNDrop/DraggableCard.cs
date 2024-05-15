using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [field: SerializeField] public bool ResetDraggableTransform { get; private set; }
    public DraggableCard ChildDraggable { get; private set; }
    public DraggableCard ParentDraggable { get; private set; }
    public Transform ParentTransform { get; set; }
    public Transform RootTransform { get; set; }
    public bool IsActivable { get; set; } = true;

    private Image _image;
    private Vector3 _mouseOffset;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        ParentTransform = transform.parent;
        transform.SetParent(RootTransform ?? transform.root);
        ParentDraggable = null;
        _image.raycastTarget = false;
        _mouseOffset = transform.position - Input.mousePosition;

        if (ParentTransform.TryGetComponent(out DropZone dropZone))
        {
            dropZone.ResetCurrentDraggable();
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        transform.position = Input.mousePosition + _mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        _image.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        UpdateCurrentDraggable();

        GameObject droppedObject = eventData.pointerDrag;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (newDraggable.ParentDraggable == this) return; // Ensure you can't drop cards on children

        if (ChildDraggable == null)
        {
            SetNewDraggable(newDraggable);
        }

        if (ResetDraggableTransform) ChildDraggable.transform.localPosition = Vector3.zero;
    }

    public void UpdateCurrentDraggable()
    {
        if (ChildDraggable != null && ChildDraggable.ParentTransform != transform && ChildDraggable.ParentTransform != transform.root)
        {
            ResetCurrentDraggable();
        }
    }

    public void ResetCurrentDraggable()
    {
        ChildDraggable = null;
    }

    public void SetNewDraggable(DraggableCard newDraggable)
    {
        ChildDraggable = newDraggable;
        if (ChildDraggable != null)
        {
            newDraggable.ParentDraggable = this;
            ChildDraggable.ParentTransform = transform;
            ChildDraggable.transform.SetParent(transform);
            ChildDraggable.transform.localPosition = Vector3.down * 20;
        }
    }
}
