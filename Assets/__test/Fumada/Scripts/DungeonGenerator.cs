using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator
{
    List<RoomNode> allNodeCollection = new List<RoomNode>();
    private int dungeonWidth, dungeonLength;
    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<Node> CalculateDungeon(int maxIterations, int roomWidthMin, int roomLenghtMin, float roomBottomCornerModifier, float roomTopCornerModifier, int roomOffset, int corridorWidth, Transform parentTransform, GameObject enemyPrefab)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allNodeCollection = bsp.PrepareNodesCollection(maxIterations, roomWidthMin, roomLenghtMin);
        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomLenghtMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);

        RoomNode startRoom = roomList.OrderBy(r => Vector2.Distance(Vector2.zero, new Vector2(r.BottomLeftAreaCorner.x, r.BottomLeftAreaCorner.y))).First();
        RoomNode endRoom = roomList.OrderByDescending(r => Vector2.Distance(Vector2.zero, new Vector2(r.BottomLeftAreaCorner.x, r.BottomLeftAreaCorner.y))).First();
        
        startRoom.IsStartRoom = true;
        endRoom.IsEndRoom = true;

        Vector3 startCenter = new Vector3((startRoom.BottomLeftAreaCorner.x + startRoom.TopRightAreaCorner.x) / 2, 1, (startRoom.BottomLeftAreaCorner.y + startRoom.TopRightAreaCorner.y) / 2);
        Vector3 endCenter = new Vector3((endRoom.BottomLeftAreaCorner.x + endRoom.TopRightAreaCorner.x) / 2, 1, (endRoom.BottomLeftAreaCorner.y + endRoom.TopRightAreaCorner.y) / 2);

        GameObject startMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startMarker.transform.position = startCenter;
        startMarker.GetComponent<Renderer>().material.color = Color.green;
        startMarker.transform.parent = parentTransform;
        startMarker.name = "SpawnPoint";
        
        GameObject endMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        endMarker.transform.position = endCenter;
        endMarker.GetComponent<Renderer>().material.color = Color.red;
        endMarker.transform.parent = parentTransform;

        foreach (var room in roomList)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 enemyPosition = new Vector3(
                    UnityEngine.Random.Range(room.BottomLeftAreaCorner.x, room.TopRightAreaCorner.x),
                    1,
                    UnityEngine.Random.Range(room.BottomLeftAreaCorner.y, room.TopRightAreaCorner.y));
                
                GameObject enemy = GameObject.Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
                enemy.transform.parent = parentTransform;
            }
        }

        CorridorsGenerator corridorGenerator = new CorridorsGenerator();
        var corridorList = corridorGenerator.CreateCorridor(allNodeCollection, corridorWidth);


        return new List<Node>(roomList).Concat(corridorList).ToList();
    }
}
