using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public GameObject damageTextPrefab;
    public TextMesh healthTextMesh;
    public Slider healthBar;
    public float damageTextSpawnOffset = 1.0f;
    private Renderer enemyRenderer;
    private Coroutine healthBarCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
        UpdateHealthBar();
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
            if (healthBarCoroutine == null)
            {
                healthBarCoroutine = StartCoroutine(ShowHealthBarForSeconds(5f));
            }
            else if (!IsCoroutineRunning(healthBarCoroutine))
            {
                healthBarCoroutine = StartCoroutine(ShowHealthBarForSeconds(5f));
            }

            ShowDamageText(damageAmount);
            UpdateHealthText();
            UpdateHealthBar();
            StartCoroutine(FlashRed());
        }
    }

    bool IsCoroutineRunning(Coroutine coroutine)
    {
        return coroutine != null;
    }




    void Die()
    {
        Destroy(gameObject);
    }

    void ShowDamageText(int damageAmount)
    {
        Vector3 spawnPoint = transform.position + Vector3.up * damageTextSpawnOffset;
        var go = Instantiate(damageTextPrefab, spawnPoint, Quaternion.identity);
        go.GetComponent<TextMesh>().text = damageAmount.ToString();
    }

    void UpdateHealthText()
    {
        healthTextMesh.text = currentHealth.ToString();
    }

    void UpdateHealthBar()
    {
        healthBar.value = (float)currentHealth / maxHealth;
    }

    IEnumerator FlashRed()
    {
        enemyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        enemyRenderer.material.color = Color.white;
    }

    IEnumerator ShowHealthBarForSeconds(float seconds)
    {
        healthBar.gameObject.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < seconds)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        FadeOutHealthBar();
    }

    void FadeOutHealthBar()
    {
        StartCoroutine(FadeOutHealthBarCoroutine());
    }

    IEnumerator FadeOutHealthBarCoroutine()
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        CanvasGroup canvasGroup = healthBar.GetComponent<CanvasGroup>();

        while (elapsedTime < fadeDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
        }

        canvasGroup.alpha = 0f; // Ensure alpha is set to 0 when fading is complete
        healthBar.gameObject.SetActive(false);
    }

}
