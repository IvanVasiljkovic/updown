using UnityEngine;

public class DamageToPlayer : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage to inflict on the player
    public float damageInterval = 1f; // Time interval between damage ticks
    private float nextDamageTime; // Time when the next damage tick will occur

    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Check if enough time has passed since the last damage tick
            if (Time.time >= nextDamageTime)
            {
                // Inflict damage on the player
                other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

                // Calculate the time for the next damage tick
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}
