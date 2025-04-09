using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerController : MonoBehaviour
{
    //Variables relacionadas al movimiento
    protected CharacterController controller;
    protected PlayerInventory playerInventory;
    protected PlayerHealth playerHealth;
    protected OffensiveAbility offensiveAbilityController;
    protected DefensiveAbility defensiveAbilityController;
    protected HealingAbility healingAbilityController;
    private Vector3 playerVelocity;
    private float dashCooldown = 1.5f;
    public bool isDashing = false;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;

    //Variables relacionadas al ataque
    public bool isAttacking = false;
    public bool isCastingAbility = false;

    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerHealth = GetComponent<PlayerHealth>();

        offensiveAbilityController = GetComponent<OffensiveAbility>();
        defensiveAbilityController = GetComponent<DefensiveAbility>();
        healingAbilityController = GetComponent<HealingAbility>();

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
            if (Input.GetKeyDown(KeyCode.Alpha1) && playerInventory.weapon != null)
            {
                playerInventory.selectedItemType = ItemType.Arma;
            }
            if (Input.GetKeyDown(KeyCode.Q) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva))
            {
                playerInventory.selectedItemType = ItemType.Habilidad;
                playerInventory.selectedAbilityType = AbilityType.Ofensiva;
            }
            if (Input.GetKeyDown(KeyCode.E) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva))
            {
                playerInventory.selectedItemType = ItemType.Habilidad;
                playerInventory.selectedAbilityType = AbilityType.Defensiva;
            }
            if (Input.GetKeyDown(KeyCode.R) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa))
            {
                playerInventory.selectedItemType = ItemType.Habilidad;
                playerInventory.selectedAbilityType = AbilityType.Curativa;
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

    public void comprobarEnemigosEnArea(Vector3 attackPosition, float attackRadius, float damage)
    {
        Collider[] colliders = Physics.OverlapSphere(attackPosition, attackRadius);
        HashSet<Enemigo> uniqueEnemies = new HashSet<Enemigo>();

        foreach (Collider c in colliders)
        {
            Enemigo enemigo = c.GetComponentInParent<Enemigo>();
            if (enemigo != null && !uniqueEnemies.Contains(enemigo))
            {
                uniqueEnemies.Add(enemigo);
                enemigo.takeDamage(damage);
            }
        }
    }
}
