using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Unity.VisualScripting;

[RequireComponent(typeof(Image))]
public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler
{
    [field:SerializeField] public Card Card { get; private set; }
    public DraggableCard ChildDraggable { get; private set; }
    public DraggableCard ParentDraggable { get; private set; }
    public Transform ParentTransform { get; set; }
    public Transform RootTransform { get; set; }
    public bool IsActivable { get; set; } = true;
    public Image Image => _image;

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
        _mouseOffset = transform.position - Input.mousePosition;

        BeginDragAllChildren();
    }

    public void BeginDragAllChildren()
    {
        transform.SetAsLastSibling();
        Image.raycastTarget = false;
        transform.DOComplete();
        transform.DOScale(GameManager.Instance.VisualData.CardDraggedScaleAmount, GameManager.Instance.VisualData.CardDraggedScaleTime);
        if (ChildDraggable == null) return;

        ChildDraggable.BeginDragAllChildren();
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        if (GameUI.Instance.MoonPhaseProgress.IsMoonPhaseOver)
        {
            OnEndDrag(eventData);
            eventData.pointerDrag = null;
        }

        transform.position = Input.mousePosition + _mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;
        
        EndDragAllChildren();

        if (OutOfBounds(out Vector3 position)) transform.DOMove(position, GameManager.Instance.VisualData.CardOutOfBoundsLerpTime);
    }

    public void EndDragAllChildren()
    {
        Image.raycastTarget = true;
        //transform.DOKill();
        transform.DOScale(Vector3.one, GameManager.Instance.VisualData.CardDroppedScaleTime);

        if (ChildDraggable == null) return;

        ChildDraggable.EndDragAllChildren();
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
        //UpdateCurrentDraggable();

        GameObject droppedObject = eventData.pointerDrag;
        DraggableCard newDraggable = droppedObject.GetComponent<DraggableCard>();

        if (IsDraggableInParents(newDraggable)) return;

        if (ChildDraggable == null)
        {
            TryDropNewDraggable(newDraggable);
        }
    }

    private void TryDropNewDraggable(DraggableCard newDraggable)
    {
        if (newDraggable.Card.Data.CardsThatCanBeStackedOn.Contains(Card.Data)) // Can Be Dropped
        {
            SetNewDraggable(newDraggable);
        }
        else // Cannot Be Dropped
        {
            Vector2 offset = transform.position - newDraggable.transform.position;
            offset.Normalize();
            newDraggable.transform.DOMove(transform.position + (Vector3)offset * GameManager.Instance.VisualData.CardRepelDistance, GameManager.Instance.VisualData.CardRepelTime);
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

    public void ResetCurrentDraggable(bool child = true, bool parent = true)
    {
        if (child) ChildDraggable = null;
        if (parent) ParentDraggable = null;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (!Input.GetMouseButton(0))
        //    transform.DOShakePosition(GameManager.Instance.VisualData.CardFocusShakeTime, GameManager.Instance.VisualData.CardFocusShakeForce);
    }

    public void DestroyCard(bool child = true, bool parent = true)
    {
        if (parent) ParentDraggable?.ResetCurrentDraggable(true, false);
        if (child) ChildDraggable?.ResetCurrentDraggable(false, true);
        Destroy(gameObject);
    }

    public void DoForAllChildren(Action action)
    {
        action.Invoke();

        if (ChildDraggable == null) return;

        ChildDraggable.DoForAllChildren(action);
    }

    public void DoForAllChildrenBefore(Action action)
    {
        if (ChildDraggable == null)
        {
            action.Invoke();
            return;
        }
        ChildDraggable.DoForAllChildrenBefore(action);
        action.Invoke();
    }

    public void DoForAllParents(Action action)
    {
        action.Invoke();

        if (ParentDraggable == null) return;

        ParentDraggable.DoForAllChildren(action);
    }

    public DraggableCard GetYoungestChild()
    {
        if (ChildDraggable == null) return this;

        return ChildDraggable.GetYoungestChild();
    }

    public DraggableCard GetOldestParent()
    {
        if (ParentDraggable == null) return this;

        return ParentDraggable.GetOldestParent();
    }

    public void DestroyAllChildren()
    {
        if (ChildDraggable == null)
        {
            DestroyCard();
            return;
        }
        ChildDraggable.DestroyAllChildren();
        DestroyCard();
    }
}