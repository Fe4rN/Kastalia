using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerController : MonoBehaviour
{
    // Variables relacionadas al movimiento
    protected CharacterController controller;
    protected PlayerInventory playerInventory;
    protected PlayerHealth playerHealth;

    private Vector3 playerVelocity;
    private float dashCooldown = 1.5f;
    public bool isDashing = false;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;

    // Variables relacionadas al ataque
    public bool isAttacking = false;
    public bool isCastingAbility = false;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    protected virtual void Update()
    {
        if (GameManager.instance.isPaused) return;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintValue : 1f;
        controller.Move(move * Time.deltaTime * playerSpeed * speedMultiplier);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing) StartCoroutine(Dash(move));
        }

        if (!isDashing && !isAttacking)
        {
            // Seleccionar arma
            if (Input.GetKeyDown(KeyCode.Alpha1) && playerInventory.weapon != null)
            {
                playerInventory.selectedItemType = ItemType.Arma;
            }

            // Habilidad ofensiva
            if (Input.GetKeyDown(KeyCode.Q) && playerInventory.offensiveAbility != null)
            {
                if (playerInventory.offensiveAbility.offensiveAbilityCooldown == 0)
                {
                    playerInventory.selectedItemType = ItemType.Habilidad;
                    playerInventory.selectedAbilityType = AbilityType.Ofensiva;
                }
            }

            // Habilidad defensiva
            if (Input.GetKeyDown(KeyCode.E) && playerInventory.defensiveAbility != null)
            {
                if (playerInventory.defensiveAbility.defensiveAbilityCooldown == 0)
                {
                    playerInventory.selectedItemType = ItemType.Habilidad;
                    playerInventory.selectedAbilityType = AbilityType.Defensiva;
                    playerInventory.defensiveAbility.EnableShield();
                }
            }

            // Habilidad curativa
            if (Input.GetKeyDown(KeyCode.R) && playerInventory.healingAbility != null)
            {
                if (playerInventory.healingAbility.healingAbilityCooldown == 0)
                {
                    playerInventory.selectedItemType = ItemType.Habilidad;
                    playerInventory.selectedAbilityType = AbilityType.Curativa;
                    StartCoroutine(playerInventory.healingAbility.ActivateHealingAbility());
                }
            }

            // Lanzar habilidad ofensiva si está seleccionada
            if (playerInventory.selectedItemType == ItemType.Habilidad &&
                playerInventory.selectedAbilityType == AbilityType.Ofensiva &&
                Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(playerInventory.offensiveAbility.ActivateOffensiveAbility());
            }
        }

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }
        else
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
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

    public void comprobarEnemigosEnArea(Vector3 attackPosition, float attackRadius, int damage)
    {
        Collider[] colliders = Physics.OverlapSphere(attackPosition, attackRadius);
        HashSet<EnemyHealth> uniqueEnemies = new HashSet<EnemyHealth>();

        foreach (Collider c in colliders)
        {
            EnemyHealth enemigo = c.GetComponentInParent<EnemyHealth>();
            if (enemigo != null && !uniqueEnemies.Contains(enemigo))
            {
                uniqueEnemies.Add(enemigo);
                enemigo.TakeDamage(damage);
            }
        }
    }
}
