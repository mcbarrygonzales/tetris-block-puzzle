using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private Vector2 pointerOffset;
    private BlockShape shape;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        shape = GetComponent<BlockShape>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset
        );

        pointerOffset -= rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint - pointerOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        bool placed = GridSystem.Instance.TryPlaceShape(shape);
        if (placed && !shape.isPlaced)
        {
            shape.isPlaced = true;
            shape.shapeManager.OnShapePlaced(shape);
        }
        else
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
