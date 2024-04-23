using UnityEngine;
using System.Collections; // Include the System.Collections namespace for IEnumerator

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth; // Expose currentHealth to the Inspector
    public GameObject damageTextPrefab;
    public TextMesh healthTextMesh; // Reference to the TextMesh component

    [Tooltip("Vertical offset for damage text spawn point")]
    public float damageTextSpawnOffset = 1.0f; // Exposed to the Inspector

    // Reference to the enemy's renderer component
    private Renderer enemyRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();

        // Get the Renderer component from the enemy (assuming it's a sprite renderer)
        enemyRenderer = GetComponent<Renderer>();
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
            ShowDamageText(damageAmount); // Show damage text
            UpdateHealthText(); // Update the health text after taking damage

            // Start the coroutine to flash red
            StartCoroutine(FlashRed());
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

    IEnumerator FlashRed()
    {
        // Change the enemy's color to red
        enemyRenderer.material.color = Color.red;

        // Wait for a short duration
        yield return new WaitForSeconds(0.5f); // Adjust the duration as needed

        // Change the enemy's color back to normal (white or original color)
        enemyRenderer.material.color = Color.white; // Change to the desired original color
    }
}
