using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            other.gameObject.GetComponent<Acciones>().takeDamage(20f);
            Destroy(gameObject);
        }
        if(other.CompareTag("Enemy")){
            Destroy(gameObject);
        }
    }
}
