using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array of room prefabs to choose from
    public Transform[] horizontalSpawnPoints; // Array of horizontal spawn points
    public Transform[] verticalSpawnPoints; // Array of vertical spawn points
    public GameObject[] endPoints; // Array of end points for the current room
    public int numberOfRooms = 5; // Number of rooms to spawn
    public float xOffset = 20f; // Offset for positioning rooms horizontally
    public float yOffset = 20f; // Offset for positioning rooms vertically
    public bool verticalFirst = true; // Whether to move vertically first

    private GameObject currentRoom; // Reference to the currently spawned room
    private int roomsSpawned = 0; // Counter for the number of rooms spawned
    private int horizontalIndex = 0; // Index for choosing the horizontal spawn point
    private int verticalIndex = 0; // Index for choosing the vertical spawn point

    void Start()
    {
        SpawnRoom(); // Spawn the first room
    }

    void SpawnRoom()
    {
        if (roomsSpawned >= numberOfRooms)
            return;

        // Choose a random room prefab from the array
        GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

        // Calculate position based on horizontal or vertical spawn points
        Vector3 position;
        if (verticalFirst && roomsSpawned % 2 == 0)
            position = verticalSpawnPoints[verticalIndex].position;
        else
            position = horizontalSpawnPoints[horizontalIndex].position;

        // Instantiate the room at the spawn point
        GameObject newRoom = Instantiate(roomPrefab, position, Quaternion.identity);

        // Update the spawn indexes
        if (verticalFirst && roomsSpawned % 2 == 0)
            verticalIndex = (verticalIndex + 1) % verticalSpawnPoints.Length;
        else
            horizontalIndex = (horizontalIndex + 1) % horizontalSpawnPoints.Length;

        // Set the current room to the newly spawned room
        currentRoom = newRoom;
        roomsSpawned++;
    }

    void Update()
    {
        // Check if the current room has reached any of the end points
        foreach (GameObject endPoint in endPoints)
        {
            if (endPoint.activeSelf)
            {
                // Spawn a new room
                SpawnRoom();
                break;
            }
        }
    }
}
