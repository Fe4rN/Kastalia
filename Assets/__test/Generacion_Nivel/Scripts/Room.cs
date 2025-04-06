using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room
{
    public float X1pos, X2pos, Y1pos, Y2pos;
    public float width, length;
    public float cellSize = 10f;
    public List<float> doors = new List<float>();
    public RoomType roomType;


    public Room(Vector3 origin, float minX, float maxX, float minY, float maxY, RoomType roomType)
    {
        float randomX = Random.Range(minX / 2f, maxX / 2f) * 2;
        float randomY = Random.Range(minY / 2f, maxY / 2f) * 2;

        this.X1pos = origin.x - randomX / 2;
        this.X2pos = origin.x + randomX / 2;
        this.Y1pos = origin.y - randomY / 2;
        this.Y2pos = origin.y + randomY / 2;

        this.width = randomX;
        this.length = randomY;

        this.roomType = roomType;

        Debug.Log($"Room of the type {roomType} created at {origin} with width {width} and length {length}");
    }
}

public enum RoomType{
    startRoom,
    safeRoom,
    enemyRoom,
    bossRoom
}
