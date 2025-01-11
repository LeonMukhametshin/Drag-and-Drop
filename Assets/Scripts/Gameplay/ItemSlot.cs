using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;
    private bool isFree = true;

    public bool IsFree => isFree;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || !isFree) return;

        DragDrop dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
        if (dragDrop == null) return;

        AssignItemToSlot(dragDrop, eventData.pointerDrag.GetComponent<RectTransform>());
    }

    private void AssignItemToSlot(DragDrop dragDrop, RectTransform itemRectTransform)
    {
        dragDrop.SetCurrentSlot(this);
        AlignItemToSlot(itemRectTransform);
        ResetItemPhysics(dragDrop);
        isFree = false;
    }

    private void AlignItemToSlot(RectTransform itemRectTransform)
    {
        itemRectTransform.anchoredPosition = rectTransform.anchoredPosition;
        itemRectTransform.localEulerAngles = Vector3.zero;
        ResizeItemToFitSlot(itemRectTransform);
    }

    private void ResizeItemToFitSlot(RectTransform itemRectTransform)
    {
        Vector2 slotSize = rectTransform.sizeDelta;
        Vector2 originalSize = itemRectTransform.sizeDelta;

        float scaleFactor = Mathf.Min(slotSize.x / originalSize.x, slotSize.y / originalSize.y);
        itemRectTransform.sizeDelta = originalSize * scaleFactor;
    }

    private void ResetItemPhysics(DragDrop dragDrop)
    {
        Rigidbody2D itemRigidbody = dragDrop.GetComponent<Rigidbody2D>();
        if (itemRigidbody == null) return;

        itemRigidbody.velocity = Vector2.zero;
        itemRigidbody.angularVelocity = 0;
        itemRigidbody.isKinematic = true;
    }

    public void OnItemRemoved()
    {
        isFree = true;
    }
}