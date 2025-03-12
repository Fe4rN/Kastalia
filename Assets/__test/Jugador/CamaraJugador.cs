using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    private Transform player;
    private Transform cam;
    private Vector3 offset = new Vector3(0,5,-10);

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
       cam.position = player.position + offset; 
    }
}
