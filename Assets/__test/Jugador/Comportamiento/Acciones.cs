using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acciones : MonoBehaviour
{
    //Variables relacionadas al movimiento
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float dashCooldown = 1.5f;
    private bool isDashing = false;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;

    //Variables relacionadas al ataque
    private bool isAttacking = false;
    [SerializeField] private float attackrange = 6.0f;
    private float attackCooldown = 1.5f;
    private Vector3 attackDamageOffset = new Vector3(0, 0, 1);

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintValue : 1f;
        controller.Move(move * Time.deltaTime * playerSpeed * speedMultiplier);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing) StartCoroutine(Dash(move));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isAttacking) StartCoroutine(SwordAttack());
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator Dash(Vector3 move)
    {

        float startTime = Time.time;
        isDashing = true;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(move * dashSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(dashCooldown - dashTime);
        isDashing = false;
    }



    private void comprobarEnemigosEnArea()
    {
        Vector3 attackPosition = controller.transform.position + controller.transform.forward * attackDamageOffset.z + controller.transform.up * attackDamageOffset.y + controller.transform.right * attackDamageOffset.x;
        Collider[] colliders = Physics.OverlapSphere(attackPosition, attackrange);
        HashSet<Enemigo> uniqueEnemies = new HashSet<Enemigo>();

        foreach (Collider c in colliders)
        {
            Enemigo enemigo = c.GetComponentInParent<Enemigo>();
            if (enemigo != null && !uniqueEnemies.Contains(enemigo))
            {
                uniqueEnemies.Add(enemigo);
                enemigo.takeDamage(20f);
            }
        }
    }
    IEnumerator SwordAttack()
    {
        if (isAttacking) yield break;
        isAttacking = true;
        comprobarEnemigosEnArea();
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    //Para debugging del area de ataque
    private void OnDrawGizmosSelected()
    {
        if (controller == null) return;
        Vector3 attackPosition = controller.transform.position + controller.transform.forward * attackDamageOffset.z + controller.transform.up * attackDamageOffset.y + controller.transform.right * attackDamageOffset.x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackrange);
    }
}
