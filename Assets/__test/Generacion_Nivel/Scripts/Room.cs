using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    private int X1pos, X2pos, Y1pos, Y2pos;
    [SerializeField] private int minX, maxX, minY, maxY;
    public int width, length;
    public float cellSize = 1f;
    public float cellQuantity = 10f;
    public int probability = 50;

    public bool isSafeRoom = false;
    public bool isHostileRoom = false;
    public bool isSpawnRoom = false;
    public bool isFinalRoom = false;

    [SerializeField]
    GameObject floorTilePrefab, wallTilePrefab, cornerWallTilePrefab, doorTilePrefab;

    void Start()
    {
        CreateRoom(new Vector2Int(0, 0), minX, maxX, minY, maxY);
        InstatiateRoom(new Vector2Int(0, 0));
        GenerateEnemies(new Dictionary<string, GameObject>(), 5);
    }

    public void CreateRoom(Vector2Int origin, int minX, int maxX, int minY, int maxY)
    {

        int randomX = UnityEngine.Random.Range(minX / 2, maxX / 2) * 2;
        int randomY = UnityEngine.Random.Range(minY / 2, maxY / 2) * 2;

        X1pos = origin.x - randomX / 2;
        X2pos = origin.x + randomX / 2;
        Y1pos = origin.y - randomY / 2;
        Y2pos = origin.y + randomY / 2;

        width = randomX;
        length = randomY;

        Debug.Log($"Room created at {origin} with width {width} and length {length}");
    }

    public void InstatiateRoom(Vector2Int position)
    {

        GameObject roomParent = new GameObject("Room");
        roomParent.transform.position = new Vector3(position.x, 0, position.y);

        Dictionary<string, Transform> cells = new Dictionary<string, Transform>();

        for (int i = Y1pos; i <= Y2pos; i++)
            for (int j = X1pos; j <= X2pos; j++)
            {
                Vector3 pos = new Vector3(position.x + i * cellSize - Y1pos, 0, position.y + j * cellSize - X1pos);

                //Esquina superior izquierda
                if (i == Y1pos && j == X1pos)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Esquina superior derecha
                if (i == Y1pos && j == X2pos)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, 90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Esquina inferior izquierda
                if (i == Y2pos && j == X1pos)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, -90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Esquina inferior derecha
                if (i == Y2pos && j == X2pos)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, 180);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Pared izquierda
                if (i != Y1pos && i != Y2pos && j == X1pos)
                {
                    Transform w = Instantiate(wallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Pared superior
                if (i == Y1pos && j != X1pos && j != X2pos)
                {
                    Transform w = Instantiate(wallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, 90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Pared derecha
                if (i != Y1pos && i != Y2pos && j == X2pos)
                {
                    Transform w = Instantiate(wallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, 180);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Pared inferior
                if (i == Y2pos && j != X1pos && j != X2pos)
                {
                    Transform w = Instantiate(wallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, -90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                //Suelos
                if (i != Y1pos && i != Y2pos && j != X1pos && j != X2pos)
                {
                    Transform w = Instantiate(floorTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    cells.Add($"{i},{j}", w);
                    continue;
                }
            }
        Debug.Log($"Generadas {cells.Count} casillas");
    }

    private void GenerateEnemies(Dictionary<String, GameObject> enemyPool, int enemyCount)
    {
        float randomXSpawn = Random.Range(X1pos, X2pos);
        float randomYSpawn = Random.Range(Y1pos, Y2pos);

        for (int i  = 0; i < enemyCount; i++)
        {
            R
        }
    }
}
