using UnityEngine;

public class PosicionCursor : MonoBehaviour
{
    private Transform player;
    private LayerMask layerMask;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        layerMask = ~LayerMask.GetMask("Player");
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 worldPoint = hit.point;
            Vector3 Yremoved = new Vector3(worldPoint.x, player.position.y, worldPoint.z);
            player.LookAt(Yremoved);
            Debug.Log(Yremoved);
        }
    }
}
