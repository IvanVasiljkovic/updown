using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject weaponHolder; // Empty GameObject to hold the weapon
    private Items selectedItem;

    // Method to set the selected item
    public void SetSelectedItem(Items item)
    {
        selectedItem = item;
        UpdateWeapon();
    }

    // Method to update the weapon display
    private void UpdateWeapon()
    {
        // Remove existing weapon
        foreach (Transform child in weaponHolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new weapon if there's a selected item
        if (selectedItem != null)
        {
            GameObject weapon = Instantiate(selectedItem.prefab, weaponHolder.transform);
            weapon.transform.localPosition = Vector3.zero; // Adjust position as needed
            weapon.transform.localRotation = Quaternion.identity; // Adjust rotation as needed
        }
    }

    // Method to use the selected item
    void Update()
    {
        if (selectedItem != null)
        {
            // Handle using the selected item (e.g., attack)
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                UseSelectedItem();
            }
        }
    }

    private void UseSelectedItem()
    {
        // Example: handle weapon attack
        if (selectedItem != null)
        {
            Debug.Log("Using item: " + selectedItem.name);

            // Add your weapon usage logic here
            // For example, if it's a sword, play an attack animation, detect enemies in range, etc.
            if (selectedItem.name == "Sword")
            {
                // Example sword attack logic
                // Play attack animation
                // Detect and damage enemies in range
            }
        }
    }
}
