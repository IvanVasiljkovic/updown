using System.Collections;
using UnityEngine;

public class PlayerMovement2DUpDown : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    [Header("Abilities")]
    public float dashDistance = 5f;
    public float dashCooldown = 2f;
    private float dashCooldownTimer;
    public float teleportCooldown = 5f;
    private float teleportCooldownTimer;

    [Header("Power-Ups")]
    private float originalSpeed;
    public float speedBoostMultiplier = 1.5f;
    public float speedBoostDuration = 5f;
    private bool isSpeedBoosted = false;

    [Header("Shooting")]
    public float bulletSpeed = 10f; // Define bullet speed here
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float shootCooldownTimer = 0f;
    private bool canShoot = true;
    public float shootCooldownDuration = 1f;

    [Header("Audio")]
    // Placeholder for audio clips like dashSound, teleportSound, etc.

    private Camera cam;

    private PlayerMovement2DUpDown movementScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        originalSpeed = moveSpeed;

        // Get the PlayerMovement2DUpDown script attached to the same GameObject
        movementScript = GetComponent<PlayerMovement2DUpDown>();
    }


    [SerializeField] private Transform weaponTransform;
    void Update()
    {
        if (!canShoot)
        {
            shootCooldownTimer -= Time.deltaTime;
            if (shootCooldownTimer <= 0)
            {
                canShoot = true;
            }
        }

        HandleInput();
        HandleCooldowns();
        HandleWeaponsAndAbilities();
        HandleShooting();

        // Get mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Face left if the mouse is on the left side of the player, otherwise face right
        if (mousePosition.x < transform.position.x)
        {
            FaceLeft();
        }
        else
        {
            FaceRight();
        }
    }



    void FaceLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1); // Flip the player sprite horizontally to face left
    }

    void FaceRight()
    {
        transform.localScale = new Vector3(1, 1, 1); // Do not flip the player sprite to face right
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        if (canShoot)
        {
            // Calculate the direction from the player to the mouse position
            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - firePoint.position).normalized;

            // Instantiate a bullet at the fire point position and rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // Calculate the rotation angle based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Set the rotation of the bullet to face towards the mouse direction
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Set the velocity of the bullet to make it travel towards the mouse direction
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = direction * bulletSpeed; // Use bulletSpeed here

            // Start the shoot cooldown
            canShoot = false;
            shootCooldownTimer = shootCooldownDuration;
        }
    }



    // Placeholder method for playing shooting sound
    void PlayShootSound() { /* Add Audio Source logic here */ }

    void FixedUpdate()
    {
        MovePlayer();
    }

    public IEnumerator DashTowardsMouse()
    {
        Vector3 targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 dashDestination = transform.position + direction * dashDistance;

        float t = 0f;
        float dashDuration = 0.7f; // Adjust the duration of the dash

        while (t < dashDuration)
        {
            t += Time.deltaTime;

            // Calculate the lerp factor using a curve for smooth acceleration and deceleration
            float lerpFactor = Mathf.SmoothStep(0f, 1f, t / dashDuration);

            // Interpolate the speed using the lerp factor, making it faster
            float currentSpeed = Mathf.Lerp(moveSpeed * 3f, moveSpeed, lerpFactor);

            // Move towards destination with the interpolated speed
            transform.position = Vector3.MoveTowards(transform.position, dashDestination, currentSpeed * Time.deltaTime);

            yield return null;
        }
    }




    void HandleInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTimer <= 0)
        {
            StartCoroutine(StartDashTimer());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(StartDashTimer());
            if (dashCooldownTimer <= 0)
            {
                StartCoroutine(movementScript.DashTowardsMouse(dashDistance));
                dashCooldownTimer = dashCooldown;
            }
        }
    }

    IEnumerator StartDashTimer()
    {
        float holdDuration = 0f;
        while (holdDuration < 3f)
        {
            holdDuration += Time.deltaTime;
            yield return null;
        }

        // Change the dash distance after holding for 3 seconds
        dashDistance = 45f;
        Debug.Log("Dash distance increased after holding for 3 seconds.");

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Reset dash distance back to 15
        dashDistance = 15f;
        Debug.Log("Dash distance reset.");
    }

    public IEnumerator DashTowardsMouse(float distance)
    {
        Vector3 targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 dashDestination = transform.position + direction * distance;

        float t = 0f;
        float dashDuration = 0.7f; // Adjust the duration of the dash

        while (t < dashDuration)
        {
            t += Time.deltaTime;

            // Calculate the lerp factor using a curve for smooth acceleration and deceleration
            float lerpFactor = Mathf.SmoothStep(0f, 1f, t / dashDuration);

            // Interpolate the speed using the lerp factor, making it faster
            float currentSpeed = Mathf.Lerp(moveSpeed * 3f, moveSpeed, lerpFactor);

            // Move towards destination with the interpolated speed
            transform.position = Vector3.MoveTowards(transform.position, dashDestination, currentSpeed * Time.deltaTime);

            yield return null;
        }

        // After the dash is completed, set dashDistance back to 15
        dashDistance = 15f;
    }


    void MovePlayer()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleCooldowns()
    {
        dashCooldownTimer -= Time.deltaTime;
        teleportCooldownTimer -= Time.deltaTime;

        if (isSpeedBoosted && speedBoostDuration <= 0)
        {
            moveSpeed = originalSpeed;
            isSpeedBoosted = false;
        }
        else if (isSpeedBoosted)
        {
            speedBoostDuration -= Time.deltaTime;
        }
    }

    void HandleWeaponsAndAbilities()
    {
        // Teleport
        if (Input.GetKeyDown(KeyCode.F) && teleportCooldownTimer <= 0)
        {
            TeleportToCursor();
            teleportCooldownTimer = teleportCooldown;
            // PlayTeleportSound();
        }

        // Activate Speed Boost
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateSpeedBoost();
            // PlayPowerUpSound();
        }
    }

    void TeleportToCursor()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Ensure we don't change the Z-axis
        transform.position = mousePos;
        // Optionally add teleport effect (particle system or flash)
    }

    void ActivateSpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            moveSpeed *= speedBoostMultiplier;
            isSpeedBoosted = true;
            speedBoostDuration = 5f;
        }
    }

    // Placeholder methods for playing sounds
    void PlayDashSound() { /* Add Audio Source logic here */ }
    void PlayTeleportSound() { /* Add Audio Source logic here */ }
    void PlayPowerUpSound() { /* Add Audio Source logic here */ }
}