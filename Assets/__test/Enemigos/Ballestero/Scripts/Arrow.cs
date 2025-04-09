using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField] private float TTL = 3f;
    private Rigidbody rb;
    private bool hasHit = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyArrowAfterTime(TTL));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        hasHit = true;
        Debug.Log("Arrow hit: " + other.name);
        PlayerHealth PlayerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
        EnemyHealth EnemyHealth = other.gameObject.GetComponentInParent<EnemyHealth>();
        if (PlayerHealth)
        {
            Debug.Log("PlayerHealth component found, applying damage.");
            PlayerHealth.takeDamage(20f);
        }
        else if (EnemyHealth)
        {
            Debug.Log("EnemyHealth component found, applying damage.");
            EnemyHealth.TakeDamage(20);
        } 
        else
        {
            Debug.Log("No health component found, ignoring hit.");
        }
        StickToTarget(other.transform);
    }

    private void StickToTarget(Transform target)
    {
        Debug.Log("Sticking to target: " + target.name);
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
