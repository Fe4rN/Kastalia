using UnityEngine;
using System.Collections.Generic;


public class RoomRenderer : MonoBehaviour
{
    public GameObject floorTilePrefab, wallTilePrefab, cornerWallTilePrefab, chestTilePrefab;
    public void RenderRoom(Room room, Vector3 originPosition, int id)
    {
        GameObject roomParent = new GameObject($"Room {id}");

        Dictionary<string, Transform> cells = new Dictionary<string, Transform>();

        int widthTiles = Mathf.RoundToInt(room.width);
        int lengthTiles = Mathf.RoundToInt(room.length);
        float cellSize = room.cellSize;

        float roomWidth = widthTiles / cellSize;
        float roomLength = lengthTiles / cellSize;

        Vector3 roomOrigin = new Vector3(
                            originPosition.x - roomWidth / 2f,
                            0,
                            originPosition.z - roomLength / 2f
                        );


        roomParent.transform.position = new Vector3(roomOrigin.x, 0, roomOrigin.z);

        for (int i = 0; i < lengthTiles; i++)
        {
            for (int j = 0; j < widthTiles; j++)
            {
                Vector3 pos = new Vector3(roomOrigin.x + j * cellSize, 0, roomOrigin.z + i * cellSize);

                bool isLeft = j == 0;
                bool isRight = j == widthTiles - 1;
                bool isTop = i == 0;
                bool isBottom = i == lengthTiles - 1;

                #region Esquinas
                if (isTop && isLeft)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                if (isTop && isRight)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, -90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                if (isBottom && isLeft)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, 90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                if (isBottom && isRight)
                {
                    Transform w = Instantiate(cornerWallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, 180);
                    cells.Add($"{i},{j}", w);
                    continue;
                }
                #endregion

                #region Paredes
                if (isLeft || isRight)
                {
                    Transform w = Instantiate(wallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, isRight ? -90 : 90);
                    cells.Add($"{i},{j}", w);
                    continue;
                }

                if (isTop || isBottom)
                {
                    Transform w = Instantiate(wallTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    w.Rotate(Vector3.up, isBottom ? 180 : 0);
                    cells.Add($"{i},{j}", w);
                    continue;
                }
                #endregion

                //Suelos y/o cofres
                if (room.roomType == RoomType.safeRoom && i == lengthTiles / 2 && j == widthTiles / 2)
                    {
                        Transform c = Instantiate(chestTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                        cells.Add($"{i},{j}", c);
                        continue;
                    }
                else
                {
                    Transform f = Instantiate(floorTilePrefab, pos, Quaternion.identity, roomParent.transform).transform;
                    cells.Add($"{i},{j}", f);
                }
            }
        }

        Debug.Log($"Room instantiated with {cells.Count} cells.");
    }
}
