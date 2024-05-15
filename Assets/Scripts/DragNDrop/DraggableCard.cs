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

    [SerializeField] private Vector3 _parentCardOffset;
    [SerializeField, Range(0, 1)] private float _cardFollowLerp;

    private Image _image;
    private Vector3 _mouseOffset;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        //transform.SetParent(RootTransform ?? transform.root);
        if (ParentDraggable != null) ParentDraggable.ChildDraggable = null;
        ParentDraggable = null;
        ParentTransform = transform.parent;
        _image.raycastTarget = false;
        _mouseOffset = transform.position - Input.mousePosition;

        transform.SetAsLastSibling();
        // HERE MAKE CHILD PLAY LAST LINE RECURSIVELY (SetAsLastSibling loop)

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

        if (newDraggable.ParentDraggable == this) // HERE CONTINUEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
        {
            //newDraggable.ChildDraggable;
            return; // Ensure you can't drop cards on children
        }

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
            //ChildDraggable.transform.SetParent(transform);
        }
    }

    private void Update()
    {
        if (ChildDraggable != null) ChildDraggable.transform.transform.position = Vector3.Lerp(ChildDraggable.transform.transform.position, transform.transform.position + _parentCardOffset, _cardFollowLerp);
    }

}
