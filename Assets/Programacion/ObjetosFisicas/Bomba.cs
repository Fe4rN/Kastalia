using UnityEngine;
using System.Collections;

public class Bomba : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float directHitDamage = 5f; // Reduced from 10f
    [SerializeField] private float explosionDamage = 30f; // Increased from 20f

    [Header("Proximity Explosion")]
    [SerializeField] private float proximityRadius = 3f;
    [SerializeField] private float explosionDelay = 5f;

    [SerializeField] private GameObject explosionRadiusIndicatorPrefab;
    [SerializeField] private GameObject dangerRadiusIndicatorPrefab;

    private Rigidbody rb;
    private bool hasHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        hasHit = true;
        rb.linearVelocity = Vector3.zero;
        StartCoroutine(startTicking(explosionDelay));

        PlayerHealth playerHealth = other.gameObject.GetComponentInParent<PlayerHealth>();
        EnemyHealth enemyHealth = other.gameObject.GetComponentInParent<EnemyHealth>();
        if (playerHealth)
        {
            playerHealth.takeDamage(directHitDamage);
        }
        else if (enemyHealth)
        {
            enemyHealth.TakeDamage(Mathf.CeilToInt(directHitDamage));
        }
    }

    private IEnumerator startTicking(float time)
    {
        //ShowDangerArea(time);
        yield return new WaitForSeconds(time);
        DealExplosionDamage(transform.position);
        ShowExplosionRadius();
    }

    // Código que muestra el área de peligro, no funciona como se esperaba

    // private void ShowDangerArea(float time){
    //     if (dangerRadiusIndicatorPrefab)
    //     {
    //         GameObject effect = Instantiate(
    //             dangerRadiusIndicatorPrefab,
    //             transform.position,
    //             Quaternion.identity
    //         );
    //         effect.transform.localScale = new Vector3(proximityRadius, .01f, proximityRadius);
    //         Destroy(effect, time);
    //     }
    // }

    private void ShowExplosionRadius()
    {
        if (explosionRadiusIndicatorPrefab)
        {
            GameObject indicator = Instantiate(
                explosionRadiusIndicatorPrefab,
                transform.position,
                Quaternion.identity
            );
            float scale = proximityRadius * 2f;
            indicator.transform.localScale = new Vector3(scale, scale, scale);
            DestroyImmediate(gameObject);

            Destroy(indicator, .5f);
        }
    }

    private void DealExplosionDamage(Vector3 bombPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(bombPosition, proximityRadius);

        foreach (Collider collider in colliders)
        {
            if (Physics.Linecast(bombPosition, collider.transform.position, out RaycastHit hit))
            {
                if (hit.collider == collider)
                {
                    collider.GetComponentInParent<PlayerHealth>()?.takeDamage(explosionDamage);
                    collider.GetComponentInParent<EnemyHealth>()?.TakeDamage(Mathf.CeilToInt(explosionDamage));
                }
            }
        }
    }
}