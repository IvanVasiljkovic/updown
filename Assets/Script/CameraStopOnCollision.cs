using UnityEngine;
using Cinemachine;

public class CameraStopOnCollision : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    public Collider2D roomCollider; // Assign this in the inspector

    private void Start()
    {
        // Find the Cinemachine virtual camera in the scene
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("Cinemachine virtual camera not found in the scene.");
        }

        // Assign the roomCollider to the virtual camera's confiner
        if (virtualCamera != null && roomCollider != null)
        {
            CinemachineConfiner confiner = virtualCamera.GetComponent<CinemachineConfiner>();
            if (confiner == null)
            {
                confiner = virtualCamera.gameObject.AddComponent<CinemachineConfiner>();
            }
            confiner.m_BoundingShape2D = roomCollider;
            confiner.InvalidatePathCache();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Stop the camera by disabling its Collider component
            virtualCamera.enabled = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Resume the camera by enabling its Collider component
            virtualCamera.enabled = true;
        }
    }
}
