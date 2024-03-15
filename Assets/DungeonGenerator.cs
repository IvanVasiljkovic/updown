 using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Start,
    Normal,
    Boss,
    Special // Add more types as needed
}

[System.Serializable]
public class RoomPrefab
{
    public RoomType type;
    public GameObject prefab;
}

[System.Serializable]
public class Room
{
    public Vector2Int location;
    public RoomType type;
    public Room parent; // Used for connecting rooms

    public Room(Vector2Int location, RoomType type)
    {
        this.location = location;
        this.type = type;
    }
}

public class DungeonGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int totalRooms = 20;
    public float roomSpacing = 15f;
    public Vector2Int mapSize = new Vector2Int(50, 50);

    [Header("Room Settings")]
    [SerializeField] private List<RoomPrefab> roomPrefabs = new List<RoomPrefab>();

    private List<Room> rooms = new List<Room>();
    private List<Vector2Int> occupiedPositions = new List<Vector2Int>();

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // Reset the map if re-generating
        rooms.Clear();
        occupiedPositions.Clear();

        // Create starting room
        CreateRoom(Vector2Int.zero, RoomType.Start);

        // Generate other rooms
        for (int i = 1; i < totalRooms; i++)
        {
            CreateRoom(GetRandomPosition(), i == totalRooms - 1 ? RoomType.Boss : RoomType.Normal); // Last room is the boss
        }

        // Instantiate rooms and hallways
        foreach (Room room in rooms)
        {
            RoomPrefab prefabInfo = roomPrefabs.Find(p => p.type == room.type);
            if (prefabInfo != null && prefabInfo.prefab != null)
            {
                Instantiate(prefabInfo.prefab, new Vector3(room.location.x * roomSpacing, room.location.y * roomSpacing, 0), Quaternion.identity);
            }

            // Connect rooms with hallways if they have a parent
            if (room.parent != null)
            {
                CreateHallway(room.parent.location, room.location);
            }
        }
    }

    private void CreateRoom(Vector2Int position, RoomType type)
    {
        Room newRoom = new Room(position, type);
        rooms.Add(newRoom);

        if (rooms.Count > 1) // Not the first room
        {
            Room previousRoom = rooms[rooms.Count - 2]; // Get the previous room

            // Calculate the position of the new room's door relative to the previous room's door
            Vector2Int doorPosition = new Vector2Int(
                position.x - previousRoom.location.x,
                position.y - previousRoom.location.y
            );

            // Set the new room's parent to the previous room
            newRoom.parent = previousRoom;

            // Add the door position to the new room's location to get the absolute position of the door
            Vector2Int absoluteDoorPosition = new Vector2Int(
                position.x + doorPosition.x,
                position.y + doorPosition.y
            );

            // Add the absolute door position to occupied positions
            occupiedPositions.Add(absoluteDoorPosition);
        }

        // Add the new room's position to occupied positions
        occupiedPositions.Add(position);
    }


    private Vector2Int GetRandomPosition()
    {
        Vector2Int position;
        do
        {
            int x = Random.Range(-mapSize.x / 2, mapSize.x / 2);
            int y = Random.Range(-mapSize.y / 2, mapSize.y / 2);
            position = new Vector2Int(x, y);
        }
        while (occupiedPositions.Contains(position) || IsPositionTooClose(position, Mathf.FloorToInt(roomSpacing)));
        return position;
    }

    private void CreateHallway(Vector2Int start, Vector2Int end)
    {
        // Convert Vector2Int to Vector2 for normalization
        Vector2 direction = ((Vector2)(end - start)).normalized;
        int length = Mathf.Max(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y));

        for (int i = 0; i <= length; i++)
        {
            // Use direction to create points between start and end, convert back to Vector2Int for positioning
            Vector2 point = new Vector2(start.x, start.y) + direction * i;
            Vector2Int hallwayPosition = new Vector2Int(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));

            GameObject hallwaySegment = new GameObject("Hallway");
            hallwaySegment.transform.position = new Vector3(hallwayPosition.x * roomSpacing, hallwayPosition.y * roomSpacing, 0);
            hallwaySegment.transform.localScale = new Vector3(roomSpacing, roomSpacing, 1); // Adjust as needed
            SpriteRenderer renderer = hallwaySegment.AddComponent<SpriteRenderer>(); // Add your sprite renderer or mesh here
                                                                                     // Set renderer properties as needed, e.g., sprite
                                                                                     // Add additional components as needed, e.g., colliders
        }
    }

    private bool IsPositionTooClose(Vector2Int position, int minDistance)
    {
        foreach (var occupiedPosition in occupiedPositions)
        {
            if (Vector2Int.Distance(occupiedPosition, position) < minDistance)
            {
                return true; // Position is too close to another room
            }
        }
        return false; // Position is fine
    }
}