using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon; // UI Image to display the item icon
    public Items item; // The item currently in this slot
    public bool isSelected = false; // Is this slot selected?

    // Add an item to the slot
    public void AddItem(Items newItem)
    {
        item = newItem;
        if (item != null && icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
    }

    // Clear the slot
    public void ClearSlot()
    {
        item = null;
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }
    }

    // Select the slot
    public void SelectSlot()
    {
        isSelected = true;
        // Update the UI to show this slot is selected (e.g., change the color or add an outline)
        icon.color = Color.yellow; // Example highlight
    }

    // Deselect the slot
    public void DeselectSlot()
    {
        isSelected = false;
        // Update the UI to show this slot is not selected
        icon.color = Color.white; // Default color
    }
}
