using UnityEngine;
using System.Collections;

public class Bomba : MonoBehaviour
{
    [Header("Basic Settings")]
    [SerializeField] private float TTL = 3f;
    [SerializeField] private float pushForce = 2f;
    [SerializeField] private float gravityScale = 2f;
    [SerializeField] private float bounceFactor = 0.5f; // How much it bounces off player
    
    [Header("Damage Settings")]
    [SerializeField] private float directHitDamage = 5f; // Reduced from 10f
    [SerializeField] private float explosionDamage = 30f; // Increased from 20f
    
    [Header("Proximity Explosion")]
    [SerializeField] private bool enableProximityDamage = true;
    [SerializeField] private float proximityRadius = 3f;
    [SerializeField] private float explosionDelay = 1f;
    [SerializeField] private GameObject explosionEffect;
    
    private Rigidbody rb;
    private bool hasHit = false;
    private Acciones playerActions;
    private Vector3 hitVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerActions = FindObjectOfType<Acciones>();
        StartCoroutine(DestroyArrowAfterTime(TTL));
    }

    void FixedUpdate()
    {
        // Apply additional gravity
        rb.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        
        hasHit = true;
        hitVelocity = rb.linearVelocity;

        // Handle player hit
        Acciones hitActions = other.GetComponentInParent<Acciones>();
        if (hitActions != null)
        {
            HandlePlayerHit(other.transform, hitActions);
        }
        else // Handle environment hit
        {
            HandleEnvironmentHit();
        }
    }

    private void HandlePlayerHit(Transform playerTransform, Acciones actions)
    {
        // Apply damage and push
        actions.takeDamage(directHitDamage);
        PushTarget(playerTransform);
        
        // Bounce off player
        Vector3 bounceDirection = Vector3.Reflect(hitVelocity.normalized, playerTransform.forward);
        rb.linearVelocity = bounceDirection * hitVelocity.magnitude * bounceFactor;
        
        // Enable explosion countdown
        if (enableProximityDamage)
        {
            StartCoroutine(ExplosionCountdown());
        }
    }

    private void HandleEnvironmentHit()
    {
        // Enable explosion countdown when hitting environment
        if (enableProximityDamage)
        {
            StartCoroutine(ExplosionCountdown());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);

        // Check for player in explosion radius
        if (playerActions != null)
        {
            float distance = Vector3.Distance(transform.position, playerActions.transform.position);
            if (distance <= proximityRadius)
            {
                playerActions.takeDamage(explosionDamage);
                PushTarget(playerActions.transform);
                
                if (explosionEffect != null)
                {
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                }
            }
        }
        
        Destroy(gameObject);
    }

    private void PushTarget(Transform target)
    {
        Rigidbody targetRb = target.GetComponentInParent<Rigidbody>();
        if (targetRb != null)
        {
            Vector3 pushDirection = (target.position - transform.position).normalized;
            targetRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    private IEnumerator DestroyArrowAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (enableProximityDamage)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, proximityRadius);
        }
    }
}