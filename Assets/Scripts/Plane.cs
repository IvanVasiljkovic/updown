using UnityEngine;

public class Plane : MonoBehaviour
{
    public Transform door; // Reference to the door object in the room

    // Use this for initialization
    void Start()
    {
        // Ensure that a door is assigned
        if (door == null)
        {
            Debug.LogError("Door reference not set in room: " + gameObject.name);
        }
    }
}
