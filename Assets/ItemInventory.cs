using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public InventorySlot[] slots; // An array of inventory slots

    // Adds an item to the inventory
    public bool Add(Items item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null) // Check for an empty slot
            {
                slots[i].AddItem(item);
                return true;
            }
        }
        // No empty slots
        Debug.Log("No empty slots available");
        return false;
    }
}
