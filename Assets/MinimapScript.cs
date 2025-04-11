using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if(!player){
            if (FindFirstObjectByType<CharacterController>() == null) return;
            else{ player = FindFirstObjectByType<CharacterController>().transform; }
        }
        Vector3 position = player.position;
        transform.position = new Vector3(position.x, 10, position.z);
    }
}
