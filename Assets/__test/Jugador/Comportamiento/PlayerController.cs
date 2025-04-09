using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Variables relacionadas al movimiento
    private CharacterController controller;
    private PlayerInventory playerInventory;
    private PlayerHealth playerHealth;
    private OffensiveAbility offensiveAbilityController;
    private DefensiveAbility defensiveAbilityController;
    private HealingAbility healingAbilityController;
    private Vector3 playerVelocity;
    private float dashCooldown = 1.5f;
    public bool isDashing = false;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.1f;

    //Variables relacionadas al ataque
    public bool isAttacking = false;
    [SerializeField] private float attackrange = 6.0f;
    [SerializeField] private float bowReach = 30.0f;
    private float arrowRadius = 0.1f;
    private float attackCooldown = 1.5f;
    private Vector3 attackDamageOffset = new Vector3(0, 0, 1);

    // –––––––––––––––––––––––––––––––-------------------------------
    //variables: de funcionalidad de espada
    // –––––––––––––––––––––––––––––––-------------------------------
    [SerializeField] private float chargeMultiplier = 3f;
    private bool isFullyCharged = false;
    [SerializeField] private float empujeFuerza = 10f;
    private bool isChargingSword = false;
    private float chargeTime = 0f;
    private float chargeRequiredTime = 1f;
    private bool isRightMouseDown = false;


    // –––––––––––––––––––––––––––––––-------------------------------

    // Inventario
    public string currentlySelected;

    public float offensiveAbilityCooldown = 0;
    public float defensiveAbilityCooldown = 0;
    public int defensiveAbilityHits = 0;
    public float healingAbilityCooldown = 0;
    public bool isCastingAbility = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInventory = GetComponent<PlayerInventory>();
        playerHealth = GetComponent<PlayerHealth>();
        
        offensiveAbilityController = GetComponent<OffensiveAbility>(); 
        defensiveAbilityController = GetComponent<DefensiveAbility>();
        healingAbilityController = GetComponent<HealingAbility>();

    }

    void Update()
    {
        if (GameManager.instance.isPaused) return;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintValue : 1f;
        controller.Move(move * Time.deltaTime * playerSpeed * speedMultiplier);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing) StartCoroutine(Dash(move));
        }
        if (currentlySelected == "Ofensiva"
            && playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva)
            && offensiveAbilityCooldown == 0)
        {
            StartCoroutine(offensiveAbilityController.offensiveAbility());
            Debug.Log("Habilidad Ofensiva");
        }
        else if (currentlySelected == "Defensiva" && playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva)
        && defensiveAbilityCooldown == 0)
        {
            defensiveAbilityController.enableShield();
            Debug.Log("Habilidad Defensiva");
        }
        else if (currentlySelected == "Curativa"
            && playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa)
            && healingAbilityCooldown == 0)
        {
            StartCoroutine(healingAbilityController.healingAbility());
            Debug.Log("Habilidad Curativa");
        }

        if (Input.GetKeyDown(KeyCode.Tab)) PrintLoadout();
        if (Input.GetKeyDown(KeyCode.Alpha1) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva) && offensiveAbilityCooldown == 0 && currentlySelected != "Ofensiva")
        {
            currentlySelected = "Ofensiva";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva) && defensiveAbilityCooldown == 0 && currentlySelected != "Defensiva")
        {
            currentlySelected = "Defensiva";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa) && healingAbilityCooldown == 0 && currentlySelected != "Curativa")
        {
            currentlySelected = "Curativa";
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
        Collider[] colliders = Physics.OverlapSphere(attackPosition, attackrange);
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

    //Para visualizar el rango de ataque de la espada (debugging)
    private void OnDrawGizmosSelected()
    {
        if (controller == null) return;
        Vector3 attackPosition = controller.transform.position + controller.transform.forward * attackDamageOffset.z + controller.transform.up * attackDamageOffset.y + controller.transform.right * attackDamageOffset.x;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackrange);
    }

    public void PrintLoadout() //Debugging
    {
        // Debug.Log($"Character: {GameManager.instance.currentCharacter}");
        // Debug.Log($"Weapon: {(equippedWeapon != null ? equippedWeapon : "None")}");

        // foreach (var ability in playerInventory.equippedAbilities.Values)
        // {
        //     Debug.Log($"Ability: {ability.abilityName} ({ability.abilityType})");
        // }
    }
}
