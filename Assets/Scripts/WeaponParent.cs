using System.Collections;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;
    private int currentWeaponIndex = 0; // Index of the current weapon
    public GameObject[] weapons; // Array of weapon game objects

    private void Start()
    {
        // Ensure only the first weapon is active at the start
        SetActiveWeapon(currentWeaponIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            // Trigger the attack animation
            Attack();
        }

        // Check for weapon cycling input
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.Alpha1))
        {
            CycleWeapons(1); // Cycle to the next weapon
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.Alpha2))
        {
            CycleWeapons(-1); // Cycle to the previous weapon
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

    private void CycleWeapons(int direction)
    {
        // Update current weapon index based on direction
        currentWeaponIndex += direction;

        // Ensure index stays within bounds
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Length - 1;
        }
        else if (currentWeaponIndex >= weapons.Length)
        {
            currentWeaponIndex = 0;
        }

        // Set the active weapon
        SetActiveWeapon(currentWeaponIndex);
    }

    private void SetActiveWeapon(int index)
    {
        // Deactivate all weapons
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Activate the selected weapon
        weapons[index].SetActive(true);
    }
}
