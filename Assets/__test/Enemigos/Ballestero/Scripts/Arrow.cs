using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private float TTL = 3f;
    private Rigidbody rb;
    private bool hasHit = false;
    private float damage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyArrowAfterTime(TTL));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        hasHit = true;
        PlayerHealth PlayerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
        EnemyHealth EnemyHealth = other.gameObject.GetComponentInParent<EnemyHealth>();
        if (PlayerHealth)
        {
            PlayerHealth.takeDamage(damage);
        }
        else if (EnemyHealth)
        {
            EnemyHealth.TakeDamage(Mathf.CeilToInt(damage));
        } 
        else
        {
            //Implementacion futura para objetos
        }
        StickToTarget(other.transform);
    }

    public void setDamage(float damage){
        this.damage = damage;
    }

    private void StickToTarget(Transform target)
    {
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        transform.SetParent(target);
    }

    IEnumerator DestroyArrowAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
