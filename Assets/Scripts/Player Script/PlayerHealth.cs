using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHearts = 3; // Starting number of hearts for the player
    public int maxHearts = 5; // Maximum number of hearts displayed in UI
    private int remainingHearts; // Remaining hearts of the player

    public float invulnerabilityTime = 2f; // Duration of invulnerability after taking damage
    private bool isInvulnerable = false;   // Flag to indicate if the player is currently invulnerable
    private SpriteRenderer spriteRenderer;  // Reference to the sprite renderer component

    void Start()
    {
        remainingHearts = startingHearts; // Set remaining hearts to starting hearts when the game starts
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Function to take damage
    public void TakeDamage(int damageAmount)
    {
        if (!isInvulnerable)
        {
            remainingHearts--; // Decrease remaining hearts by one

            // Check if the player has run out of hearts
            if (remainingHearts <= 0)
            {
                Die(); // Call the Die function to load the game over scene
                return;
            }

            // Start blinking and become invulnerable
            StartCoroutine(BlinkRoutine());
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    // Coroutine for the blinking effect
    IEnumerator BlinkRoutine()
    {
        float blinkInterval = 0.1f; // Interval between blinks
        float blinkDuration = invulnerabilityTime; // Duration of blinking (same as invulnerability time)

        while (blinkDuration > 0f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle sprite visibility
            yield return new WaitForSeconds(blinkInterval);
            blinkDuration -= blinkInterval;
        }

        spriteRenderer.enabled = true; // Ensure sprite is visible after blinking
    }

    // Coroutine for invulnerability period
    IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true; // Set invulnerable flag to true
        yield return new WaitForSeconds(invulnerabilityTime); // Wait for invulnerability time
        isInvulnerable = false; // Set invulnerable flag to false after the duration is over
    }

    // Function to handle player death and load game over scene
    void Die()
    {
        Debug.Log("Player has died!");
        // Load the game over scene here
        SceneManager.LoadScene("GameOverScene"); // Replace "GameOverScene" with the actual name of your game over scene
    }

    // Function to calculate remaining hearts for UI display
    public int GetRemainingHearts()
    {
        return remainingHearts;
    }
}
