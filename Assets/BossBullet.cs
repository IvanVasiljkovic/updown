using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public int damageAmount = 25; // Damage dealt by the bullet
    public float lifespan = 5f; // How long before the arrow is automatically destroyed

    void Start()
    {
        // Automatically destroy the bullet after its lifespan ends
        Destroy(gameObject, lifespan);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player GameObject
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Check if the player has a PlayerHealth component
            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(damageAmount);
            }

            // Destroy the bullet after hitting the player
            Destroy(gameObject);
        }
    }
}
