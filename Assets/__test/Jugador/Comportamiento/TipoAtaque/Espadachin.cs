using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Espadachin : MonoBehaviour
{
    [SerializeField] float attackrange = 2f;
    [SerializeField] float chargeRequiredTime = 2f;
    public int chargeMultiplier = 2;
    public float chargeTime = 0f;
    public bool isChargingSword = false;
    public bool isFullyCharged = false;
    public bool isRightMouseDown = false;
    private PlayerController controller;
    private Vector3 attackDamageOffset;
    [SerializeField] private float empujeFuerza = 5f;
    public float attackCooldown = 0.5f;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }


    // –––––––––––––––––––––––––––––––-------------------------------
    //FUNCIONALIDAD: carga de la espada
    // –––––––––––––––––––––––––––––––-------------------------------
    public IEnumerator ChargeSword()
    {
        if (isChargingSword)
        {
            Debug.Log("[CHARGE] Ya se está cargando, ignorando nueva carga");
            yield break;
        }

        Debug.Log("[CHARGE] Iniciando corrutina de carga");
        isChargingSword = true;
        isFullyCharged = false;
        chargeTime = 0f;

        while (isRightMouseDown && chargeTime < chargeRequiredTime)
        {
            chargeTime += Time.deltaTime;
            Debug.Log($"[CHARGE] Progreso: {(chargeTime / chargeRequiredTime * 100).ToString("F0")}%");
            yield return null;
        }

        if (chargeTime >= chargeRequiredTime)
        {
            isFullyCharged = true;
            Debug.Log("[CHARGE] ¡Carga COMPLETADA! Listo para ataque cargado");
        }
        else
        {
            Debug.Log("[CHARGE] Carga CANCELADA antes de completarse");
        }

        isChargingSword = false;
        Debug.Log("[CHARGE] Corrutina de carga finalizada");
    }
    // –––––––––––––––––––––––––––––––-------------------------------
    //FUNCIONALIDAD: Empuje del Enemigo tras ser dañado
    // –––––––––––––––––––––––––––––––-------------------------------
    private void EmpujarEnemigo(GameObject enemigo, float fuerzaEmpuje)
    {
        // Calcular dirección del empuje (desde jugador hacia enemigo)
        Vector3 direccionEmpuje = (enemigo.transform.position - transform.position).normalized;
        direccionEmpuje.y = 0; // Mantenemos el empuje en el plano horizontal

        // Aplicar el empuje directamente al transform
        StartCoroutine(EmpujeSuave(enemigo.transform, direccionEmpuje, fuerzaEmpuje));

        Debug.Log($"[EMPUJE] Empujando enemigo {enemigo.name} con fuerza {fuerzaEmpuje}");
    }

    private IEnumerator EmpujeSuave(Transform objetivo, Vector3 direccion, float fuerza)
    {
        float duracionEmpuje = 0.5f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionEmpuje)
        {
            // Calcular progreso (de 1 a 0)
            float progreso = 1 - (tiempoTranscurrido / duracionEmpuje);

            // Mover el enemigo
            objetivo.position += direccion * fuerza * progreso * Time.deltaTime;

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
    }


    // –––––––––––––––––––––––––––––––-------------------------------
    //FUNCIONALIDAD: Ataque de la Espada
    // –––––––––––––––––––––––––––––––-------------------------------
    public IEnumerator SwordAttack(int damage)
    {
        if (controller.isAttacking) yield break;
        controller.isAttacking = true;

        Vector3 attackPosition = controller.transform.position + controller.transform.forward * attackDamageOffset.z +
                            controller.transform.up * attackDamageOffset.y +
                            controller.transform.right * attackDamageOffset.x;

        // Comprobar enemigos en área y aplicar daño y empuje
        Collider[] colliders = Physics.OverlapSphere(attackPosition, attackrange);
        HashSet<EnemyHealth> uniqueEnemies = new HashSet<EnemyHealth>();

        foreach (Collider c in colliders)
        {
            EnemyHealth enemigo = c.GetComponentInParent<EnemyHealth>();
            if (enemigo != null && !uniqueEnemies.Contains(enemigo))
            {
                uniqueEnemies.Add(enemigo);

                // Aplicar daño
                enemigo.TakeDamage(damage);

                // Aplicar empuje
                float fuerzaEmpujeActual = isFullyCharged ? empujeFuerza * 1.5f : empujeFuerza;
                EmpujarEnemigo(enemigo.gameObject, fuerzaEmpujeActual);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        controller.isAttacking = false;
    }
}
