using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damageAmount = 10f;
    public float attackDuration = 0.5f; // Duration of the attack animation
    public float attackCooldown = 0.8f; // Cooldown between attacks
    private Animator animator;
    private bool isAttacking;
    private bool damageDealt; // Flag to track if damage has been dealt during the current attack
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown; // Start with a negative value to allow immediate attack
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time - lastAttackTime > attackCooldown)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack");
            // Start dealing damage during the attack animation
            StartAttack();
            lastAttackTime = Time.time; // Update the last attack time
        }
    }

    // Method to start dealing damage during the attack animation
    void StartAttack()
    {
        isAttacking = true;
        damageDealt = false; // Reset the damageDealt flag for the new attack
        Invoke("EndAttack", attackDuration); // End the attack after a certain duration
    }

    // Method to end the attack
    void EndAttack()
    {
        isAttacking = false;
        damageDealt = false; // Reset the damageDealt flag when the attack ends
    }

    // Collision detection with enemies
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking && !damageDealt)
        {
            // Check if the collided object is an enemy
            if (other.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage((int)damageAmount);
                    damageDealt = true; // Set the damageDealt flag to true to prevent multiple damage applications in the same attack
                }
            }
        }
    }
}
