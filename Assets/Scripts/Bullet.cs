using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }

}