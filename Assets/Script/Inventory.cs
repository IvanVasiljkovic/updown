using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public int quantity;
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public GameObject inventoryUI;

    void Start()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        Debug.Log("Toggling Inventory");
        inventoryUI?.SetActive(!inventoryUI.activeSelf);
    }

    public void AddItem(string itemName, Sprite icon, int quantity)
    {
        InventoryItem existingItem = items.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            InventoryItem newItem = new InventoryItem
            {
                itemName = itemName,
                icon = icon,
                quantity = quantity
            };

            items.Add(newItem);
        }

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        // Implement logic to update your UI with the current inventory state
        // For example, you can use events, UI controllers, or other methods
    }
}
