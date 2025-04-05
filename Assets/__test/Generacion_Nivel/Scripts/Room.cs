using UnityEngine;
using Random = UnityEngine.Random;

public class Room
{
    public float X1pos, X2pos, Y1pos, Y2pos;
    public float width, length;
    public float cellSize = 1f;
    public float cellQuantity = 10f;
    public int probability = 50;

    public bool isSafeRoom = false;
    public bool isHostileRoom = false;
    public bool isSpawnRoom = false;
    public bool isFinalRoom = false;


    public Room(Vector3 origin, float minX, float maxX, float minY, float maxY)
    {
        float randomX = Random.Range(minX / 2f, maxX / 2f) * 2;
        float randomY = Random.Range(minY / 2f, maxY / 2f) * 2;

        X1pos = origin.x - randomX / 2;
        X2pos = origin.x + randomX / 2;
        Y1pos = origin.y - randomY / 2;
        Y2pos = origin.y + randomY / 2;

        width = randomX;
        length = randomY;

        Debug.Log($"Room created at {origin} with width {width} and length {length}");
    }
}
