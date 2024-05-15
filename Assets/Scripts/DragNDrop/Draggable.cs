using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ParentTransform { get; set; }
    public DropZone ParentDropZone { get; set; }
    public Transform RootTransform { get; set; }
    public bool IsActivable { get; set; } = true;

    private Image _image;
    //[SerializeField] private AudioClip _dragEndSound;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        ParentTransform = transform.parent;
        transform.SetParent(RootTransform ?? transform.root);
        ParentDropZone = null;
        _image.raycastTarget = false;

        if (ParentTransform.TryGetComponent(out DropZone dropZone))
        {
            dropZone.ResetCurrentDraggable();
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsActivable) return;

        //transform.SetParent(ParentTransform);

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
