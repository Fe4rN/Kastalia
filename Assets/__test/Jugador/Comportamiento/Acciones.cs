using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Acciones : MonoBehaviour
{
    //Atributos del jugador
    public float vidaMaxima = 100f;
    public float vidaActual;
    //private bool estaVivo = true;
    private float tiempoImmunidad = 1.5f;
    private bool inmunidad = false;
    
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
    [SerializeField] private float bowReach = 30.0f;
    private float arrowRadius = 0.1f;
    private float attackCooldown = 1.5f;
    private Vector3 attackDamageOffset = new Vector3(0, 0, 1);

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if(GameManager.instance.isPaused) return;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintValue : 1f;
        controller.Move(move * Time.deltaTime * playerSpeed * speedMultiplier);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing) StartCoroutine(Dash(move));
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isAttacking) {
                if(GameManager.instance.characterIndex == 1){
                    StartCoroutine(SwordAttack());
                    Debug.Log("Ataque Espada");
                }else if(GameManager.instance.characterIndex == 2){
                    StartCoroutine(bowAttack());
                    Debug.Log("Ataque Arco");
                }
            }
        }
        if (vidaActual <= 0) Die();

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


    public void takeDamage(float damage)
    {
        if (inmunidad) return;
        vidaActual -= damage;
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            Die();
        }
        StartCoroutine(ActivarInmunidad());
    }

    public void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
        LevelManager.instance.isLevelLoaded = false;
        GameManager.instance.RestartGame();
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

    private void trazarRayoArco()
    {
        PosicionCursor cursorToWorld = GetComponent<PosicionCursor>();
        Debug.Log(cursorToWorld.lookPoint);
        Vector3 direccion = (cursorToWorld.lookPoint - transform.position).normalized;

        Ray rayo = new Ray(transform.position, direccion);
        RaycastHit hit;

        if (Physics.SphereCast(rayo, arrowRadius, out hit, bowReach))
        {
            Enemigo enemigo = hit.collider.GetComponentInParent<Enemigo>();
            Debug.DrawLine(transform.position, hit.point, Color.red, 1f); // Debugging line
            if(enemigo){
                enemigo.takeDamage(20f);
            }
            
        }
        else
        {
            Vector3 endPoint = transform.position + direccion * bowReach; // End of the ray
            Debug.DrawLine(transform.position, endPoint, Color.green, 1f); // Debug line when nothing is hit
            Debug.Log("Arrow max range reached.");
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

    IEnumerator bowAttack()
    {
        if (isAttacking) yield break;
        isAttacking = true;
        trazarRayoArco();
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;

    }

    IEnumerator ActivarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoImmunidad);
        inmunidad = false;
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
