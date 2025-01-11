using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image backgroundImage;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Rigidbody2D rigidbody2D;
    private ItemSlot currentSlot;

    private Vector2 originalSize;

    private void Awake()
    {
        InitializeComponents();
        HideBackgroundImage();
        SaveOriginalSize();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartDragging();
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragItem(eventData.delta);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StopDragging();
    }

    private void InitializeComponents()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void HideBackgroundImage()
    {
        if (backgroundImage != null)
        {
            backgroundImage.enabled = false;
        }
    }

    private void StartDragging()
    {
        canvasGroup.blocksRaycasts = false;

        ResetToOriginalSize();
        EnableBackgroundImage();
       
        if (rigidbody2D != null)
        {
            rigidbody2D.isKinematic = true;
        }
    }

    private void DragItem(Vector2 delta)
    {
        rectTransform.anchoredPosition += delta / canvas.scaleFactor;
        ClampToScreenBounds();
        currentSlot = null;
    }

    private void StopDragging()
    {
        canvasGroup.blocksRaycasts = true;
        DisableBackgroundImage();

        if (currentSlot == null && rigidbody2D != null)
        {
            rigidbody2D.isKinematic = false;
        }
        else
        {
            currentSlot?.OnItemRemoved();
        }
    }

    private void EnableBackgroundImage()
    {
        if (backgroundImage != null)
        {
            backgroundImage.enabled = true;
        }
    }

    private void DisableBackgroundImage()
    {
        if (backgroundImage != null)
        {
            backgroundImage.enabled = false;
        }
    }

    private void ClampToScreenBounds()
    {
        if (canvas == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);

        Vector3 minBounds = canvasCorners[0];
        Vector3 maxBounds = canvasCorners[2];

        Vector3 clampedPosition = rectTransform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);

        rectTransform.position = clampedPosition;
    }

    public void SetCurrentSlot(ItemSlot slot)
    {
        currentSlot = slot;
    }

    private void SaveOriginalSize()
    {
        originalSize = rectTransform.sizeDelta;
    }

    private void ResetToOriginalSize()
    {
        rectTransform.sizeDelta = originalSize;
    }
}