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
        Acciones acciones = other.gameObject.GetComponentInParent<Acciones>();
        if (acciones != null)
        {
            Debug.Log("Acciones component found, applying damage.");
            StickToTarget(other.transform);
            acciones.takeDamage(20f);
        } else {
            Debug.Log("No Acciones component found, destroying arrow.");
            Destroy(gameObject);
        }
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
