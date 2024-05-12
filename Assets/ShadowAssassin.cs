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
    public GameObject projectilePrefab;
    public GameObject shadowTrapPrefab;
    public int damage = 20;
    public LayerMask environmentLayer;

    private float teleportTimer;
    private float attackTimer;
    private float dashTimer;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private bool isDashing = false;
    private bool isCooldown = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        teleportTimer = teleportInterval;
        attackTimer = attackInterval;
        dashTimer = dashCooldown;
        StartCoroutine(VisibilityToggle());
    }

    void Update()
    {
        if (!isDashing)
        {
            NormalMovement();
        }

        HandleAttack();
        CooldownManagement();
        SetTraps();
    }

    void NormalMovement()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, step);

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
        spriteRenderer.enabled = false;
        isDashing = true;

        // Explicitly convert transform.position to Vector2 to avoid ambiguity with Vector3
        Vector2 startPos = transform.position;
        Vector2 playerPos = player.position;  // Convert player's position to Vector2 if necessary

        // Calculate target position by converting Vector3 to Vector2 and ensuring all operations are between Vector2 objects
        Vector2 targetPos = playerPos + (playerPos - startPos).normalized * -3f; // Normalize the difference and move behind the player

        float dashDuration = 0.5f;
        float dashElapsed = 0;

        while (dashElapsed < dashDuration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, dashElapsed / dashDuration);
            dashElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;  // Ensure the position is set as Vector2
        spriteRenderer.enabled = true;
        isDashing = false;
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
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDashing)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    void CooldownManagement()
    {
        if (isCooldown)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                dashTimer = dashCooldown;
                isCooldown = false;
            }
        }
    }

    void SetTraps()
    {
        if (Random.Range(0, 1000) < 5) // Random chance to set a trap
        {
            Vector2 trapPos = (Vector2)player.position + Random.insideUnitCircle * 2; // Convert player.position to Vector2
            RaycastHit2D hit = Physics2D.Raycast(trapPos, Vector2.down, 0.1f, environmentLayer);
            if (hit.collider == null)
            {
                Instantiate(shadowTrapPrefab, trapPos, Quaternion.identity);
            }
        }
    }
}
