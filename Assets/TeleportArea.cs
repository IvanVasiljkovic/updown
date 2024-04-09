using UnityEngine;

public class TeleportArea : MonoBehaviour
{
    public Vector3 teleportDestination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        player.transform.position = teleportDestination;
    }
}
