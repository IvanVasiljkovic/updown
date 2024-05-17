using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventorySlot inventorySlot;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // Make the item slightly transparent to indicate it's being dragged
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<InventorySlot>() != null)
        {
            InventorySlot targetSlot = eventData.pointerEnter.GetComponent<InventorySlot>();

            if (targetSlot != null)
            {
                if (targetSlot.slotType == InventorySlot.SlotType.Hotbar)
                {
                    targetSlot.AddItem(inventorySlot.item); // Add to hotbar slot
                    // Optionally keep the item in the inventory too
                }
                else
                {
                    // Swap items between slots
                    Items targetItem = targetSlot.item;
                    targetSlot.AddItem(inventorySlot.item);
                    inventorySlot.AddItem(targetItem);
                }
            }
        }

        // Return to original position if no valid drop target
        rectTransform.anchoredPosition = originalPosition;
        transform.SetParent(inventorySlot.transform);
    }
}
