using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 25f; // Damage dealt by the arrow
    public float lifespan = 5f; // How long before the arrow is automatically destroyed

    void Start()
    {
        Destroy(gameObject, lifespan); // Automatically destroy the arrow after its lifespan ends
    }

    // Check for collisions with other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is an enemy
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            // Deal damage to the enemy
            enemy.TakeDamage(damage);
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
