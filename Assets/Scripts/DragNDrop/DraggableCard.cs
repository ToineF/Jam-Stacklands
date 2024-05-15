using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [field:SerializeField] public Card Card { get; private set; }
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

        //transform.SetParent(RootTransform ?? transform.root);
        if (ParentDraggable != null) ParentDraggable.ChildDraggable = null;
        ParentDraggable = null;
        ParentTransform = transform.parent;
        _image.raycastTarget = false;
        _mouseOffset = transform.position - Input.mousePosition;
        transform.DOKill();

        ShowBeforeEverything();
    }

    public void ShowBeforeEverything()
    {
        transform.SetAsLastSibling();
        if (ChildDraggable == null) return;

        ChildDraggable.ShowBeforeEverything();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        if (GameUI.Instance.MoonPhaseProgress.IsMoonPhaseOver)
        {
            eventData.pointerDrag = null;
        }

        transform.position = Input.mousePosition + _mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        _image.raycastTarget = true;

        if (OutOfBounds(out Vector3 position)) transform.DOMove(position, GameManager.Instance.VisualData.CardOutOfBoundsLerpTime);
    }

    private bool OutOfBounds(out Vector3 position)
    {
        Vector2 min = GameManager.Instance.MinDragNDropZone.position;
        Vector2 max = GameManager.Instance.MaxDragNDropZone.position;
        position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        return (transform.position.x < min.x || transform.position.x > max.x || transform.position.y < min.y || transform.position.y > max.y);
    }

    public void OnDrop(PointerEventData eventData)
    {
        UpdateCurrentDraggable();

        GameObject droppedObject = eventData.pointerDrag;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (IsDraggableInParents(newDraggable)) return;

        if (ChildDraggable == null)
        {
            SetNewDraggable(newDraggable);
        }
    }

    public bool IsDraggableInChildren(DraggableCard draggable)
    {
        if (draggable == this) return true;
        if (ChildDraggable == null) return false;

        return ChildDraggable.IsDraggableInChildren(draggable);
    }

    public bool IsDraggableInParents(DraggableCard draggable)
    {
        if (draggable == this) return true;
        if (ParentDraggable == null) return false;

        return ParentDraggable.IsDraggableInParents(draggable);
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
            Card.CheckStack(newDraggable);
        }
    }

    private void Update()
    {
        if (ChildDraggable != null) ChildDraggable.transform.transform.position = Vector3.Lerp(ChildDraggable.transform.transform.position, transform.transform.position + GameManager.Instance?.VisualData.ParentOffset ?? Vector2.down * 20, GameManager.Instance?.VisualData.CardFollowLerp ?? 0.3f);
    }
}