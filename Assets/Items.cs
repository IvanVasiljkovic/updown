using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Items : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon = null; // The icon to represent the item in the inventory
}
