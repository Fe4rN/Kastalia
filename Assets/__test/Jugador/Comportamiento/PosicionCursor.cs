using UnityEngine;

public class PosicionCursor : MonoBehaviour
{
    public Vector3 lookPoint;
    private Transform player;
    private LayerMask layerMask;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        layerMask = ~LayerMask.GetMask("Player", "Paredes, Enemies");
    }
    void Update()
    {
        if(GameManager.instance.isPaused) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 worldPoint = hit.point;
            Vector3 Yremoved = new Vector3(worldPoint.x, player.position.y, worldPoint.z);
            lookPoint = Yremoved;
            player.LookAt(Yremoved);
        }
    }
}
