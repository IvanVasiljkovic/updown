using System.Collections.Generic;
using UnityEngine;

public class ShadowPeopleBehavior : MonoBehaviour
{
    public float speed = 3.5f;
    public float attackRange = 1.5f;
    public float retreatRange = 0.75f;  // When closer than this range, consider retreating
    public float attackCooldown = 1.5f;
    public float throwableSearchRadius = 5f;  // Radius to look for throwable objects
    public AudioClip[] movementSounds;
    public AudioClip attackSound;
    public AudioClip[] randomChatter;
    public AudioClip throwSound;  // Sound for throwing objects
    public AudioSource audioSource;
    public GameObject alertSignalPrefab; // Visual alert when coordinating attacks
    public LayerMask throwableLayer;  // Layer to identify throwable objects

    private Transform player;
    private float lastAttackTime = 0;
    private Animator animator;
    private static List<ShadowPeopleBehavior> squad = new List<ShadowPeopleBehavior>();
    private GameObject currentThrowable;  // Currently targeted throwable object

    void Awake()
    {
        animator = GetComponent<Animator>();
        squad.Add(this);
    }

    void OnDestroy()
    {
        squad.Remove(this);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("PlayRandomChatter", 2f, 7f);
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && distanceToPlayer > retreatRange)
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= retreatRange)
        {
            RetreatFromPlayer();
        }
        else
        {
            MoveTowardsPlayer();
        }

        //if (ShouldCallForHelp())
        //{
        //    AlertSquad();
        //}

        SearchAndThrow();
    }

    //bool ShouldCallForHelp()
    //{
    //    // Example condition: Call for help when health is low or outnumbered
    //    // This is a simple placeholder; customize it according to your game's logic
    //   /* return health <= 50 || squad.Count < 3;*/ // Assuming 'health' is defined and 'squad' is the list of all shadow people
    //}


    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        animator.SetTrigger("Move");
        PlayMovementSound();
    }

    void AttackPlayer()
    {
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(attackSound);
    }

    void RetreatFromPlayer()
    {
        Vector2 retreatDirection = (transform.position - player.position).normalized;
        transform.position += (Vector3)retreatDirection * speed * Time.deltaTime;
        animator.SetTrigger("Retreat");
    }

    void AlertSquad()
    {
        foreach (var member in squad)
        {
            if (member != this)
            {
                member.SupportAttack();
                Instantiate(alertSignalPrefab, member.transform.position, Quaternion.identity);
            }
        }
    }

    void SupportAttack()
    {
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * 2 * Time.deltaTime);
        }
    }

    void SearchAndThrow()
    {
        if (currentThrowable == null)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, throwableSearchRadius, throwableLayer);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Throwable"))
                {
                    currentThrowable = hit.gameObject;
                    break;
                }
            }
        }
        else
        {
            ThrowObjectAtPlayer();
        }
    }

    void ThrowObjectAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        currentThrowable.GetComponent<Rigidbody2D>().AddForce(direction * 500);  // Adjust force as necessary
        audioSource.PlayOneShot(throwSound);
        currentThrowable = null;  // Reset after throwing
    }

    void PlayMovementSound()
    {
        if (!audioSource.isPlaying)
        {
            AudioClip clip = movementSounds[Random.Range(0, movementSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void PlayRandomChatter()
    {
        if (randomChatter.Length > 0)
        {
            AudioClip clip = randomChatter[Random.Range(0, randomChatter.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
