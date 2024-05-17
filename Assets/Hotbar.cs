using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public InventorySlot[] slots; // Array of hotbar slots
    private int selectedIndex = 0; // Currently selected slot index
    private Player player; // Reference to the player script

    void Start()
    {
        player = FindObjectOfType<Player>(); // Find the player script in the scene
        if (slots.Length > 0)
        {
            SelectSlot(0); // Select the first slot by default
        }
    }

    void Update()
    {
        // Handle input for selecting hotbar slots
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(4);
        // Add more keys as needed

        // Notify the player about the selected item
        if (player != null && slots.Length > 0)
        {
            player.SetSelectedItem(slots[selectedIndex].item);
        }
    }

    void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
        {
            return;
        }

        // Deselect the previous slot
        slots[selectedIndex].DeselectSlot();

        // Select the new slot
        selectedIndex = index;
        slots[selectedIndex].SelectSlot();
    }
}
