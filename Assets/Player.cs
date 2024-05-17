using UnityEngine;

public class Player : MonoBehaviour
{
    public ItemInventory inventory;
    private ItemPickup itemInRange;

    void Update()
    {
        // Check if the player presses the "E" key
        if (Input.GetKeyDown(KeyCode.E) && itemInRange != null)
        {
            bool wasPickedUp = inventory.Add(itemInRange.item);

            if (wasPickedUp)
            {
                Destroy(itemInRange.gameObject);
                itemInRange = null; // Clear the reference after picking up
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ItemPickup itemPickup = other.GetComponent<ItemPickup>();
        if (itemPickup != null)
        {
            itemInRange = itemPickup; // Store reference to item in range
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ItemPickup itemPickup = other.GetComponent<ItemPickup>();
        if (itemPickup != null && itemPickup == itemInRange)
        {
            itemInRange = null; // Clear reference when out of range
        }
    }
}
