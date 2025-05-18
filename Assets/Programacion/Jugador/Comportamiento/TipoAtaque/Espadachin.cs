using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Espadachin : MonoBehaviour
{
    [SerializeField] float attackrange = 2f;
    [SerializeField] float chargeRequiredTime = 1f;
    public float chargeMultiplier = 3f;
    public float chargeTime = 0f;
    public bool isChargingSword = false;
    public bool isFullyCharged = false;
    public bool isRightMouseDown = false;
    private LyxController controller;
    private PlayerInventory playerInventory;
    private Animator animator;
    private Vector3 attackDamageOffset;
    [SerializeField] private float empujeFuerza = 5f;
    public float attackCooldown = 0.5f;

    void Start()
    {
        controller = GetComponent<LyxController>();
        playerInventory = GetComponent<PlayerInventory>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"[Click] ThisObject: {gameObject.name}, controller: {controller.name}, isAttacking: {controller.isAttacking}");

            if (controller.isAttacking)
            {
                Debug.Log("Attack ignored: already attacking.");
                return;
            }

            Debug.Log("Ataque ligero");
            int damage = playerInventory.weapon.damage;
            animator.SetTrigger("AtaqueLigero");
            StartCoroutine(SwordAttack(damage));
        }


        if (Input.GetMouseButtonDown(1))
        {
            isRightMouseDown = true;
            StartCoroutine(ChargeSword());
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseDown = false;

            if (isFullyCharged && !controller.isAttacking)
            {
                int baseDamage = playerInventory.weapon.damage;
                int damage = Mathf.CeilToInt(baseDamage * chargeMultiplier);
                StartCoroutine(SwordAttack(damage));
                attackCooldown = 1f;
                isFullyCharged = false;
            }
        }
    }

    public IEnumerator ChargeSword()
    {
        if (isChargingSword)
            yield break;

        isChargingSword = true;
        isFullyCharged = false;
        chargeTime = 0f;

        while (isRightMouseDown && chargeTime < chargeRequiredTime)
        {
            chargeTime += Time.deltaTime;
            yield return null;
        }

        if (chargeTime >= chargeRequiredTime)
        {
            isFullyCharged = true;
        }

        isChargingSword = false;
    }

    private void EmpujarEnemigo(GameObject enemigo, float fuerzaEmpuje)
    {
        Vector3 direccionEmpuje = (enemigo.transform.position - transform.position).normalized;
        direccionEmpuje.y = 0;
        StartCoroutine(EmpujeSuave(enemigo.transform, direccionEmpuje, fuerzaEmpuje));
    }

    private IEnumerator EmpujeSuave(Transform objetivo, Vector3 direccion, float fuerza)
    {
        float duracionEmpuje = 0.5f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionEmpuje)
        {
            float progreso = 1 - (tiempoTranscurrido / duracionEmpuje);
            objetivo.position += direccion * fuerza * progreso * Time.deltaTime;
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator SwordAttack(int damage)
    {
        controller.isAttacking = true;

        try
        {
            Vector3 attackPosition = controller.transform.position +
                                     controller.transform.forward * attackDamageOffset.z +
                                     controller.transform.up * attackDamageOffset.y +
                                     controller.transform.right * attackDamageOffset.x;

            Collider[] colliders = Physics.OverlapSphere(attackPosition, attackrange);
            HashSet<EnemyHealth> uniqueEnemies = new HashSet<EnemyHealth>();

            foreach (Collider c in colliders)
            {
                EnemyHealth enemigo = c.GetComponentInParent<EnemyHealth>();
                if (enemigo != null && !uniqueEnemies.Contains(enemigo))
                {
                    uniqueEnemies.Add(enemigo);
                    enemigo.TakeDamage(damage);

                    float fuerzaEmpujeActual = isFullyCharged ? empujeFuerza * 1.5f : empujeFuerza;
                    EmpujarEnemigo(enemigo.gameObject, fuerzaEmpujeActual);
                }
            }
            yield return new WaitForSeconds(attackCooldown);
            controller.isAttacking = false;
            attackCooldown = isFullyCharged ? 1f : 0.5f;
        }
        finally
        {
            controller.isAttacking = false;
            Debug.Log("Resetting isAttacking to false");
        }
    }
}
