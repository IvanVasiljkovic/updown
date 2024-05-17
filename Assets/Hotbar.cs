using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public InventorySlot[] slots; // Array of hotbar slots
    private int selectedIndex = 0; // Currently selected slot index

    void Start()
    {
        if (slots.Length > 0)
        {
            SelectSlot(0); // Select the first slot by default
        }
    }

    void Update()
    {
        // Handle input for selecting hotbar slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i)) // Alpha1 = KeyCode.1, Alpha2 = KeyCode.2, and so on
            {
                Debug.Log("Selected slot " + i);
                SelectSlot(i);
                break;
            }
        }

        // Handle input for using items from the hotbar
        if (Input.GetKeyDown(KeyCode.Mouse0)) UseSelectedItem();
    }


    // Select a slot in the hotbar
    void SelectSlot(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            selectedIndex = index;

            // Deselect all slots first
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].DeselectSlot();
            }

            // Select the chosen slot
            slots[selectedIndex].SelectSlot();
        }
    }

    // Use the item in the currently selected slot
    void UseSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Length)
        {
            // Get the item from the selected slot
            Items selectedItem = slots[selectedIndex].item;

            if (selectedItem != null)
            {
                // Use the item (e.g., consume, activate ability, etc.)
                Debug.Log("Using item: " + selectedItem.itemName);
            }
            else
            {
                // No item in the selected slot
                Debug.Log("No item in the selected slot.");
            }
        }
    }
}
