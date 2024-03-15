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

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the attack animation is playing and damage hasn't been dealt yet
        if (isAttacking && !damageDealt)
        {
            // Check if the collided object is an enemy
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damageAmount);
                Debug.Log("Sword collided with the enemy and dealt " + damageAmount + " damage.");

                // Check if the enemy is dead after taking damage
                if (enemy.IsDead())
                {
                    // You may want to perform additional actions here when the enemy is killed
                    Debug.Log("Enemy Killed!");
                }
            }
            damageDealt = true; // Set damageDealt to true to indicate damage has been dealt
        }
    }
}
