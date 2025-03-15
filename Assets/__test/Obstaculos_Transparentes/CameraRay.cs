using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;

    //Camera cam;
    Transform player;
    void Start()
    {
        //cam = GetComponent<Camera>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position,
        (player.position - transform.position),
        Vector3.Distance(transform.position, player.position),
        layerMask);

        if (hits.Length > 0)
        {
            foreach (var item in hits)
            {
                if (item.transform.TryGetComponent<ObstaculoTransparente>(out ObstaculoTransparente obstaculo))
                {
                    obstaculo.hitted = true;
                }
            }
        }
    }
}
