using UnityEngine;

public class CameraRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    Transform player;
    private bool playerTransformFound = false;
    void Start()
    {
        if (player) player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(LevelManager.instance.isLevelLoaded == false) return;
        if(!playerTransformFound) {
            findPlayerTransform();
            return;
        }
        RaycastHit[] hits = Physics.RaycastAll(transform.position, player.position - transform.position,
        Vector3.Distance(transform.position, player.position),layerMask);

        if(hits.Length > 0){
            foreach(var item in hits){
                Debug.Log(item.transform.name);
                if(item.transform.TryGetComponent<ObstaculoTransparente>(out ObstaculoTransparente obstaculo)){
                    obstaculo.hitted = true;
                }
            }
        }
    }

    private void findPlayerTransform(){
        player = GameObject.FindWithTag("Player").transform;
        if(player) playerTransformFound = true;
    }
}