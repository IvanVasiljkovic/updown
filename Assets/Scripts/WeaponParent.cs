using System.Collections;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Trigger the attack animation
            Attack();
        }

        // Get the position of the mouse on the screen
        Vector3 mousePosition = Input.mousePosition;
        // Set the distance of the weapon parent from the camera
        mousePosition.z = 10f; // Adjust this value if needed

        // Convert the mouse position from screen space to world space
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction from the weapon parent to the mouse position
        Vector3 direction = (mouseWorldPosition - transform.position).normalized;

        // Calculate the angle to rotate the weapon parent
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the weapon parent to face the mouse
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Flip the weapon sprite if it's on the left side of the screen
        if (mouseWorldPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void Attack()
    {
        if (!attackBlocked)
        {
            // Trigger the attack animation
            animator.SetTrigger("Attack");

            // Attack animation will trigger the damage dealing logic
            attackBlocked = true;
            StartCoroutine(DelayAttack());
        }
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }


    // Method to deal damage when the attack animation plays
    public void DealDamage()
    {
        // Insert code here to deal damage to enemies when the attack animation plays
        // You can call methods from other scripts here or directly deal damage
        Debug.Log("Dealing damage!");
    }
}
