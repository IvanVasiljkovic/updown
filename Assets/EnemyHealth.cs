using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth; // Expose currentHealth to the Inspector
    public GameObject damageTextPrefab;
    public TextMesh healthTextMesh; // Reference to the TextMesh component

    [Tooltip("Vertical offset for damage text spawn point")]
    public float damageTextSpawnOffset = 1.0f; // Exposed to the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ShowDamageText(damageAmount); // Pass the damageAmount
            UpdateHealthText(); // Update the health text after taking damage
        }
    }

    void Die()
    {
        // Implement death behavior here (e.g., play death animation, deactivate GameObject, etc.)
        Destroy(gameObject);
    }

    void ShowDamageText(int damageAmount)
    {
        // Calculate spawn point above the enemy
        Vector3 spawnPoint = transform.position + Vector3.up * damageTextSpawnOffset;

        var go = Instantiate(damageTextPrefab, spawnPoint, Quaternion.identity);
        go.GetComponent<TextMesh>().text = "" + damageAmount.ToString(); // Show the amount of health lost
    }

    void UpdateHealthText()
    {
        // Update the TextMesh component with the current health
        healthTextMesh.text = currentHealth.ToString();
    }
}