using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject FloatingTextPrefab;

    public float baseSpeed = 5f;
    public float chaseSpeedMultiplier = 1.2f; // AI moves faster when chasing
    public float maxRayDistance = 3f;
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;
    public float chaseDistance = 15f;
    public float fieldOfViewAngle = 160f;
    public int wallDetectionRays = 15;
    public float wallDetectionAngle = 180f;
    public float wanderTime = 4f;
    public float decisionInterval = 1f; // Time between new decisions when searching or wandering
    public float memoryTime = 10f;

    private Vector2 movementDirection;
    private Vector2 smoothedMovementDirection;
    private float speed;
    private float lastDecisionTime;
    private Vector2 lastKnownPlayerPosition;
    private float lastSeenPlayerTime;
    private State currentState = State.Wandering;
    private Transform playerTransform;
    public float smoothTurnTime = 0.3f; // Time taken to smoothly transition between directions
    private float smoothTurnVelocity;

    private Animator animator;
    private bool isWalking = false;
    private bool isStanding = false; // Tracks if the standing animation is playing

    private enum State { Wandering, Chasing, Searching }

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeDirection();
        speed = baseSpeed;
        currentHealth = maxHealth;

        animator = GetComponent<Animator>(); // Getting the Animator component
    }

    void Update()
    {
        CheckForPlayer();

        // Only execute movement logic if not standing
        if (!isStanding)
        {
            switch (currentState)
            {
                case State.Wandering:
                    WanderingBehavior();
                    break;
                case State.Chasing:
                    ChasingBehavior();
                    break;
                case State.Searching:
                    SearchingBehavior();
                    break;
            }

            AvoidCollisions();
            SmoothMovement();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
        if (FloatingTextPrefab)
        {
            ShowFloatingText(amount);
        }
    }

    void ShowFloatingText(float damageAmount)
    {
        var go = Instantiate(FloatingTextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = damageAmount.ToString();
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    void Die()
    {
        // Perform actions to handle enemy death, such as destroying the GameObject
        Destroy(gameObject);
    }

    void WanderingBehavior()
    {
        if (Time.time > lastDecisionTime + wanderTime)
        {
            ChangeDirection();
            lastDecisionTime = Time.time;
        }
        speed = baseSpeed;
        isWalking = true;
    }

    void ChasingBehavior()
    {
        if (CanSeePlayer() && Vector2.Distance(transform.position, playerTransform.position) <= chaseDistance)
        {
            lastKnownPlayerPosition = playerTransform.position;
            lastSeenPlayerTime = Time.time;
            movementDirection = (lastKnownPlayerPosition - (Vector2)transform.position).normalized;
            speed = baseSpeed * chaseSpeedMultiplier;
            isWalking = true;
        }
        else if (Time.time - lastSeenPlayerTime > memoryTime)
        {
            currentState = State.Searching;
        }
    }

    void SearchingBehavior()
    {
        if (Time.time - lastSeenPlayerTime > memoryTime)
        {
            currentState = State.Wandering;
        }
        else if (Time.time > lastDecisionTime + decisionInterval)
        {
            ChangeDirection();
            lastDecisionTime = Time.time;
        }
        speed = baseSpeed;
        isWalking = true;
    }

    void SmoothMovement()
    {
        smoothedMovementDirection = Vector2.Lerp(smoothedMovementDirection, movementDirection, smoothTurnTime);
        Move(smoothedMovementDirection.normalized * speed);

        if (isStanding && !animator.GetCurrentAnimatorStateInfo(0).IsName("SlimeStanding"))
        {
            // Resume movement when standing animation is finished
            isStanding = false;
        }
    }

    void Move(Vector2 direction)
    {
        // Only move if not standing
        if (!isStanding)
        {
            transform.Translate(direction * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Detect when Slime standing animation is playing
        if (other.CompareTag("SlimeStanding"))
        {
            isStanding = true;
        }
    }

    void UpdateAnimations()
    {
        // Set isWalking to true if the current state is Wandering or Chasing
        isWalking = currentState == State.Wandering || currentState == State.Chasing;

        // Update animator parameters based on the current state
        animator.SetBool("IsWalking", isWalking);
    }

    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, chaseDistance, obstacleLayer | playerLayer);
        return hit.collider != null && hit.collider.gameObject == playerTransform.gameObject;
    }

    void CheckForPlayer()
    {
        if (CanSeePlayer())
        {
            lastKnownPlayerPosition = playerTransform.position;
            lastSeenPlayerTime = Time.time;
            currentState = State.Chasing;
        }
    }

    void AvoidCollisions()
    {
        bool shouldChooseNewDirection = false;
        for (int i = 0; i < wallDetectionRays; i++)
        {
            float angle = Mathf.Lerp(-wallDetectionAngle / 2, wallDetectionAngle / 2, (float)i / (wallDetectionRays - 1));
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * movementDirection;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, maxRayDistance, obstacleLayer);
            if (hit.collider != null)
            {
                shouldChooseNewDirection = true;
                break;
            }
        }
        if (shouldChooseNewDirection && Time.time > lastDecisionTime + decisionInterval)
        {
            ChangeDirection();
            lastDecisionTime = Time.time;
        }
    }

    void ChangeDirection()
    {
        Vector2 newDirection;
        do
        {
            newDirection = Random.insideUnitCircle.normalized;
        } while (Vector2.Dot(newDirection, movementDirection) > 0.5); // Avoid minor direction changes
        movementDirection = newDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // When enemy collides with something, stop walking
        isWalking = false;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // When enemy is no longer colliding with something, resume walking
        isWalking = true;
    }
}
