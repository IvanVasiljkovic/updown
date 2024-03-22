using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 25; // Damage dealt by the arrow (changed to int)
    public float lifespan = 5f; // How long before the arrow is automatically destroyed

    private bool hasHitEnemy = false; // Flag to indicate if the arrow has already hit an enemy

    void Start()
    {
        Destroy(gameObject, lifespan); // Automatically destroy the arrow after its lifespan ends
    }

    // Check for collisions with other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHitEnemy) // Check if the arrow has already hit an enemy
            return;

        // Check if the collided object has the "Enemy" tag
        if (other.CompareTag("Enemy"))
        {
            SlimeEnemy slimeEnemy = other.GetComponent<SlimeEnemy>();
            if (slimeEnemy != null)
            {
                // Deal damage to the enemy
                slimeEnemy.TakeDamage(damage);
                Debug.Log("Hit enemy");
            }

            hasHitEnemy = true;
            Destroy(gameObject);

            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damage);
                Debug.Log("Hit enemy");
            }

            hasHitEnemy = true; // Set the flag to true to indicate that the arrow has hit an enemy
            Destroy(gameObject);
        }

        // Check if the collided object has the "Wall" tag
        if (other.CompareTag("Wall"))
        {
            // Destroy the arrow if it hits a wall
            Destroy(gameObject);
        }
    }
}
