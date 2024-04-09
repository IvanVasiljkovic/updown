using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public int quantity = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assume the player has an Inventory script attached
            Inventory inventory = other.GetComponent<Inventory>();

            if (inventory != null)
            {
                inventory.AddItem(itemName, icon, quantity);
                Destroy(gameObject); // Remove the item from the scene after pickup
            }
        }
    }
}
