using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 25f; // Damage dealt by the arrow
    public float lifespan = 5f; // How long before the arrow is automatically destroyed

    void Start()
    {
        Destroy(gameObject, lifespan); // Automatically destroy the arrow after its lifespan ends
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destroy the arrow upon hitting a wall
        }
        // Optionally, you can check for other tags if you want the arrow to interact with more objects
    }
}
