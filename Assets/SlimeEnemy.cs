using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour
{

    public int maxHealth = 100; // Maximum health of the slime enemy
    private int currentHealth; // Current health of the slime enemy
    public float invulnerabilityDuration = 0.2f; // Duration of invulnerability after taking damage

    private bool isInvulnerable = false; // Flag to indicate if the enemy is currently invulnerable


    public GameObject FloatingTextPrefab; // Prefab for displaying floating text when hit

    public float moveSpeed = 3f; // Movement speed of the enemy
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public int damageAmount = 10; // Amount of damage the enemy deals to the player

    public Animator animator; // Reference to the Animator component

    private Transform player; // Reference to the player's transform
    private bool isFollowingPlayer = false; // Flag to indicate if the enemy is currently following the player

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player GameObject and get its transform
    }

    void Update()
    {
        if (player == null)
            return;

        // Calculate the distance between the enemy and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Check if the player is within the detection range
        if (distanceToPlayer <= detectionRange)
        {
            isFollowingPlayer = true; // Set flag to indicate that the enemy is following the player

            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Set the "IsMoving" parameter of the animator to true
            if (animator != null)
                animator.SetBool("IsMoving", true);
        }
        else
        {
            isFollowingPlayer = false; // Set flag to indicate that the enemy is not following the player

            // Set the "IsMoving" parameter of the animator to false
            if (animator != null)
                animator.SetBool("IsMoving", false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isInvulnerable) // Check if the enemy is not currently invulnerable
        {
            currentHealth -= damageAmount; // Decrease current health by the damage amount

            if (currentHealth <= 0)
            {
                Die(); // Call the Die function if health drops to or below zero
            }

            if (FloatingTextPrefab != null)
            {
                ShowFloatingText(damageAmount); // Display floating text showing the damage amount
            }

            StartCoroutine(InvulnerabilityCooldown()); // Start the invulnerability cooldown
        }
    }

    IEnumerator InvulnerabilityCooldown()
    {
        isInvulnerable = true; // Set the enemy as invulnerable

        // Wait for the specified duration
        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false; // Set the enemy as vulnerable again
    }

    void Die()
    {
        // Perform actions to handle enemy death, such as destroying the GameObject
        Destroy(gameObject);
    }

    void ShowFloatingText(int damageAmount)
    {
        // Instantiate the floating text prefab at the enemy's position
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity);

        // Set the text to display the damage amount
        go.GetComponent<TextMesh>().text = damageAmount.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamage(); // Call the function to deal damage to the player
        }
    }

    void DealDamage()
    {
        // Get the PlayerHealth component from the player GameObject
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        // Check if the player has a PlayerHealth component
        if (playerHealth != null)
        {
            // Deal damage to the player
            playerHealth.TakeDamage(damageAmount);
        }
    }



    // Function to visualize the detection range in the Unity Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
