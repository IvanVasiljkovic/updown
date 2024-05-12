using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image iconImage; // Reference to the Image component where the item icon will be displayed

    private InventoryItem currentItem; // The item currently represented by this slot

    public void AddItem(InventoryItem item)
    {
        currentItem = item;
        iconImage.sprite = item.icon; // Set the sprite of the iconImage to the item's icon
        iconImage.enabled = true; // Ensure the icon is visible
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null; // Clear the sprite
        iconImage.enabled = false; // Hide the icon
    }
}
