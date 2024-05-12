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
    private bool isTakingDamage = false;
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
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            if (healthBarCoroutine != null)
                StopCoroutine(healthBarCoroutine);
            healthBarCoroutine = StartCoroutine(ShowHealthBarForSeconds(5f));
        }

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ShowDamageText(damageAmount);
            UpdateHealthText();
            UpdateHealthBar();
            StartCoroutine(FlashRed());
        }
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
        yield return new WaitForSeconds(seconds);
        healthBar.gameObject.SetActive(false);
        isTakingDamage = false;
    }
}
