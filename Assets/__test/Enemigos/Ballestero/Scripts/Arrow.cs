using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            collision.gameObject.GetComponent<Acciones>().takeDamage(20f);
        }
        Destroy(gameObject);
    }
}
