using System.Collections;
using UnityEngine;

public class ShadowAssassin : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 20f;
    public float teleportInterval = 5f;
    public float visibilityDuration = 1f;
    public float attackInterval = 3f;
    public float dashCooldown = 10f;
    public float projectileSpreadAngle = 10f;
    public GameObject projectilePrefab;
    public GameObject shadowTrapPrefab;
    public GameObject teleportEffectPrefab;
    public GameObject shadowPeoplePrefab;
    public GameObject shadowRealmPrefab;
    public AudioClip dashSound;
    public AudioClip attackSound;
    public AudioClip shadowRealmSound;
    public AudioClip summonSound;
    public AudioSource audioSource;
    public int damage = 20;
    public LayerMask environmentLayer;
    public float health = 100f;
    public float enrageThreshold = 25f;
    public float healAmount = 10f;
    public float healInterval = 20f;
    public float shadowPeopleCooldown = 3f;
    public float shadowRealmCooldown = 40f;
    public float shadowPeopleRadius = 5f;

    private float teleportTimer;
    private float attackTimer;
    private float dashTimer;
    private float healTimer;
    private float shadowPeopleTimer;
    private float shadowRealmTimer;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isDashing = false;
    private bool isCooldown = false;
    private bool isEnraged = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        teleportTimer = teleportInterval;
        attackTimer = attackInterval;
        dashTimer = dashCooldown;
        healTimer = healInterval;
        shadowPeopleTimer = shadowPeopleCooldown;
        shadowRealmTimer = shadowRealmCooldown;
        StartCoroutine(VisibilityToggle());
    }

    void Update()
    {
        if (health <= enrageThreshold && !isEnraged)
        {
            EnterEnrageMode();
        }

        if (!isDashing && Vector3.Distance(transform.position, player.position) > 3f)
        {
            NormalMovement();
        }

        HandleAttack();
        ManageCooldowns();
        SetTraps();
        HealOverTime();
        ManageShadowPeople();
        ManageShadowRealm();
    }

    void NormalMovement()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);

        teleportTimer -= Time.deltaTime;
        if (teleportTimer <= 0 && !isCooldown)
        {
            StartCoroutine(DashThroughPlayer());
            teleportTimer = teleportInterval;
            isCooldown = true;
        }
    }

    IEnumerator DashThroughPlayer()
    {
        Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(dashSound);
        spriteRenderer.enabled = false;
        isDashing = true;
        Vector3 startPos = transform.position;
        Vector3 playerPos = player.position;
        Vector3 dashDirection = (playerPos - startPos).normalized;
        Vector3 targetPos = playerPos + dashDirection * 3f;  // Extend past player

        float dashDuration = 0.5f;
        float dashElapsed = 0;

        while (dashElapsed < dashDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, dashElapsed / dashDuration);
            dashElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        spriteRenderer.enabled = true;
        isDashing = false;
        yield return new WaitForSeconds(0.5f);  // Wait half a second before disappearing
        spriteRenderer.enabled = false;  // Simulate disappearance
    }

    IEnumerator VisibilityToggle()
    {
        while (true)
        {
            yield return new WaitForSeconds(visibilityDuration);
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }

    void HandleAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && spriteRenderer.enabled)
        {
            Attack();
            attackTimer = attackInterval;
        }
    }

    void Attack()
    {
        audioSource.PlayOneShot(attackSound);
        float angleStep = projectileSpreadAngle / 3;
        float startingAngle = -projectileSpreadAngle / 2;
        for (int i = 0; i < 3; i++)
        {
            float projectileDir = startingAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, projectileDir));
            Instantiate(projectilePrefab, transform.position, rotation);
        }
    }

    void ManageShadowPeople()
    {
        shadowPeopleTimer -= Time.deltaTime;
        if (shadowPeopleTimer <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * shadowPeopleRadius;
                Instantiate(shadowPeoplePrefab, spawnPos, Quaternion.identity);
            }
            audioSource.PlayOneShot(summonSound);
            shadowPeopleTimer = shadowPeopleCooldown;
        }
    }

    void ManageShadowRealm()
    {
        shadowRealmTimer -= Time.deltaTime;
        if (shadowRealmTimer <= 0)
        {
            Instantiate(shadowRealmPrefab, player.position, Quaternion.identity);
            audioSource.PlayOneShot(shadowRealmSound);
            shadowRealmTimer = shadowRealmCooldown;
        }
    }

    void SetTraps()
    {
        if (Random.Range(0, 1000) < 5)
        {
            Vector3 trapPos = player.position + Random.insideUnitSphere * 2;
            RaycastHit2D hit = Physics2D.Raycast(trapPos, Vector3.down, 0.1f, environmentLayer);
            if (hit.collider == null)
            {
                Instantiate(shadowTrapPrefab, trapPos, Quaternion.identity);
            }
        }
    }

    void EnterEnrageMode()
    {
        isEnraged = true;
        speed *= 1.5f;
        attackInterval /= 2f;
        teleportInterval /= 2f;
        damage += 10;
        spriteRenderer.color = Color.red;
    }

    void HealOverTime()
    {
        healTimer -= Time.deltaTime;
        if (healTimer <= 0)
        {
            health += healAmount;
            healTimer = healInterval;
            if (health > 100) health = 100;  // Cap health to max
        }
    }

    void ManageCooldowns()
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            isCooldown = false;
            dashTimer = dashCooldown;
        }
    }
}
