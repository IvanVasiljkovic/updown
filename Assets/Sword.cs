using UnityEngine;

public class Sword : MonoBehaviour
{
    public float damageAmount = 10f;
    public float attackDuration = 0.5f; // Duration of the attack animation
    private Animator animator;
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack");
            // Start dealing damage during the attack animation
            StartAttack();
        }
    }

    // Method to start dealing damage during the attack animation
    void StartAttack()
    {
        isAttacking = true;
        Invoke("EndAttack", attackDuration); // End the attack after a certain duration
    }

    // Method to end the attack
    void EndAttack()
    {
        isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the attack animation is playing
        if (isAttacking)
        {
            // Check if the collided object is an enemy
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(damageAmount);

                // Check if the enemy is dead after taking damage
                if (enemy.IsDead())
                {
                    // You may want to perform additional actions here when the enemy is killed
                    Debug.Log("Enemy Killed!");
                }
            }
        }
    }
}
