using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectilesPerSecond = 3f;
    public float projectileSpeed = 5f;
    public Transform[] firePoints;
    public Vector2[] directions; // Define directions for each fire point

    private float lastShotTime;

    void Update()
    {
        ShootProjectiles();
    }

    void ShootProjectiles()
    {
        if (Time.time - lastShotTime > 1f / projectilesPerSecond)
        {
            lastShotTime = Time.time;
            for (int i = 0; i < firePoints.Length; i++)
            {
                Vector2 startPos = firePoints[i].position;
                GameObject projectile = Instantiate(projectilePrefab, startPos, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.velocity = directions[i].normalized * projectileSpeed;
            }
        }
    }
}
