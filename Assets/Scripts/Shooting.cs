using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint; // Point where the bullet will be spawned
    public GameObject[] bulletPrefabs; // Array of bullet prefabs to choose from
    public float bulletSpeed = 20f; // Speed of the bullet
    public float shootCooldown = 0.5f; // Cooldown between shots
    private float shootCooldownTimer = 0f; // Timer to track cooldown

    // Update is called once per frame
    void Update()
    {
        // Update the shoot cooldown timer
        shootCooldownTimer -= Time.deltaTime;

        // Check if the left mouse button is pressed and the cooldown is over
        if (Input.GetButtonDown("Fire1") && shootCooldownTimer <= 0f)
        {
            Shoot(); // Call the Shoot method
            shootCooldownTimer = shootCooldown; // Reset the cooldown timer
        }
    }

    void Shoot()
    {
        // Choose a random bullet prefab from the array (if the array is not empty)
        GameObject selectedBulletPrefab = bulletPrefabs.Length > 0 ? bulletPrefabs[Random.Range(0, bulletPrefabs.Length)] : null;

        // Check if a bullet prefab is selected
        if (selectedBulletPrefab != null)
        {
            // Instantiate the selected bullet prefab at the fire point position and rotation
            GameObject bullet = Instantiate(selectedBulletPrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component of the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // Check if the Rigidbody2D component is not null
            if (rb != null)
            {
                // Apply speed to the bullet in the direction of its forward vector
                rb.velocity = firePoint.right * bulletSpeed;
            }
            else
            {
                Debug.LogError("Rigidbody2D component not found on the bullet prefab.");
            }
        }
        else
        {
            Debug.LogWarning("No bullet prefabs assigned.");
        }
    }
}
