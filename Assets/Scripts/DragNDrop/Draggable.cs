using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ParentTransform { get; set; }
    public Transform RootTransform { get; set; }
    public bool IsActivable { get; set; }

    [Header("References")]
    [SerializeField] private Image _image;
    [SerializeField] private AudioClip _dragEndSound;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        ParentTransform = transform.parent;
        transform.SetParent(RootTransform ?? transform.root);
        _image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        transform.SetParent(ParentTransform);
        transform.localPosition = Vector3.zero;
        _image.raycastTarget = true;

        //MainGame.Instance?.AudioSource.PlayClip(_dragEndSound);
    }

    public void ResetLocalPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    public void SetParentTransform(Transform parent)
    {
        ParentTransform = parent;
        transform.SetParent(ParentTransform);
    }
}
