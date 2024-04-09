using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array of room prefabs to instantiate
    private GameObject currentRoom; // Reference to the current active room

    private void Start()
    {
        // Start by instantiating a random room
        SpawnRandomRooms();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered: " + other.gameObject.name); // Log the name of the collided object

        if (other.CompareTag("ExitPoint"))
        {
            Debug.Log("Player collided with ExitPoint!");
            // Teleport player to the next room
            TeleportToNextRoom();
        }
    }


    private void SpawnRandomRooms()
    {
        // Instantiate a random room prefab from the array
        GameObject newRooms = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)]);

        // Set the current room to the newly instantiated room
        currentRoom = newRooms;

        // Position the new room (can be adjusted based on your game design)
        newRooms.transform.position = Vector3.zero;

        // Position the player at the entry point of the new room
        PlacePlayerAtEntryPoint(newRooms);
    }

    private void TeleportToNextRoom()
    {
        // Destroy current room
        Destroy(currentRoom);

        // Instantiate a new random room
        SpawnRandomRooms();
    }

    private void PlacePlayerAtEntryPoint(GameObject room)
    {
        // Find entry point within the room
        Transform entryPoint = room.transform.Find("EntryPoint");

        if (entryPoint != null)
        {
            // Position the player at the entry point of the room
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (CompareTag("Player"))
            {
                player.transform.position = entryPoint.position;
            }
            else
            {
                Debug.LogError("Player GameObject not found. Make sure to tag your player GameObject with 'Player'.");
            }
        }
        else
        {
            Debug.LogError("Entry point 'EntryPoint' not found in the new room. Make sure to set an entry point named 'EntryPoint' in your room prefab.");
        }
    }

}
