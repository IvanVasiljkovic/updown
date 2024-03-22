using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    public Image[] heartIcons; // Array of heart icon images

    void Start()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        int remainingHearts = playerHealth.GetRemainingHearts();

        // Update heart icons based on player's remaining hearts
        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < remainingHearts)
            {
                // Heart icon should be visible if player has remaining hearts
                heartIcons[i].enabled = true;
            }
            else
            {
                // Heart icon should be hidden if player has no remaining hearts
                heartIcons[i].enabled = false;
            }
        }
    }

    // Update heart icons whenever player's health changes
    void Update()
    {
        UpdateHearts();
    }
}
