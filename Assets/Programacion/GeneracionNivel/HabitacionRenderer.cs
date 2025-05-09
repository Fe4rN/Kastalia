using System.Collections.Generic;
using UnityEngine;

public class HabitacionRenderer : MonoBehaviour
{
    public float prefabSize = 5f;
    [SerializeField] GameObject prefabPared;
    [SerializeField] GameObject prefabParedDecoracion;
    [SerializeField] GameObject prefabSuelo;
    [SerializeField] GameObject prefabEsquina;

    public void InstantiateRoom(Habitacion habitacion, Vector3 position){
        
        Dictionary<string, Transform> cells = new Dictionary<string, Transform>();

        for (int columnas = 0; columnas <= habitacion.numColumnas; columnas++)
        {
            for (int filas = 0; filas <= habitacion.numFilas; filas++)
            {
                Vector3 pos = new Vector3(filas * prefabSize, 0, columnas * prefabSize);

                if(filas == 0 && columnas == 0){
                    Transform w = Instantiate(prefabEsquina, pos, Quaternion.identity).transform;
                    w.Rotate(Vector3.up,180f);
                    continue;
                }

                if (filas == 0 && columnas == habitacion.numFilas){
                    Transform w = Instantiate(prefabEsquina, pos, Quaternion.identity).transform;
                    w.Rotate(Vector3.up, -90f);
                    continue;
                }

                if (filas == habitacion.numColumnas && columnas == 0){
                    Transform w = Instantiate(prefabEsquina, pos, Quaternion.identity).transform;
                    w.Rotate(Vector3.up, -90f);
                    continue;
                }

                if (filas == habitacion.numColumnas && columnas == habitacion.numColumnas){
                    Transform w = Instantiate(prefabEsquina, pos, Quaternion.identity).transform;
                    // w.Rotate(Vector3.up, -90f);
                    continue;
                }
            }
        }
    }

    private Transform InstantiateWall(Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Random.Range(0f, 1f) < 0.8f ? prefab = prefabPared : prefab = prefabParedDecoracion;
        Transform wall = Instantiate(prefab, position, rotation).transform;
        return wall;
    }

}
