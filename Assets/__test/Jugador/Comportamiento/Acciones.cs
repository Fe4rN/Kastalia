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
    
    // Nueva variable para el empuje de la espada
    [SerializeField] private float chargeMultiplier = 3f;
    private bool isFullyCharged = false;
    [SerializeField] private float empujeFuerza = 10f;
    private bool isChargingSword = false;
    private float chargeTime = 0f;
    private float chargeRequiredTime = 1f;
    private bool isRightMouseDown = false;

    // Inventario
    public string currentlySelected;
    public WeaponType allowedWeaponType;
    public Weapon equippedWeapon;
    public Dictionary<AbilityType, Ability> equippedAbilities = new Dictionary<AbilityType, Ability>();

    public float offensiveAbilityCooldown = 0;
    public float defensiveAbilityCooldown = 0;
    public int defensiveAbilityHits = 0;
    public float healingAbilityCooldown = 0;
    private bool isCastingAbility = false;            

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        vidaActual = vidaMaxima;
        if (GameManager.instance.currentCharacter == "Lyx")
        {
            allowedWeaponType = WeaponType.Espada;
        }
        else if (GameManager.instance.currentCharacter == "Dreven")
        {
            allowedWeaponType = WeaponType.Arco;
        }
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

        // Añade esto en el método Update para depuración
        if (isChargingSword)
        {
            Debug.Log($"Cargando ataque: {chargeTime.ToString("F2")} segundos");
        }

        // Manejo del click derecho para cargar el ataque de espada
        if (Input.GetKeyDown(KeyCode.Mouse1) && currentlySelected == "Espada" && equippedWeapon != null)
        {
            Debug.Log("[CHARGE] Botón derecho presionado - Iniciando carga");
            isRightMouseDown = true;
            StartCoroutine(ChargeSword());
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            Debug.Log($"[CHARGE] Botón derecho liberado - Tiempo cargado: {chargeTime.ToString("F2")}s");
            isRightMouseDown = false;
            if (isChargingSword)
            {
                StopCoroutine(ChargeSword());
                isChargingSword = false;
                chargeTime = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isAttacking && !isDashing)
            {
                if (currentlySelected == "Espada" && equippedWeapon != null)
                {
                    float damage = equippedWeapon.damage;
                    Debug.Log($"[ATTACK] Daño base: {damage}");


                    if (isFullyCharged)
                    {
                        damage *= chargeMultiplier;
                        Debug.Log($"[ATTACK] Ataque CARGADO aplicado! Daño final: {damage}");

                    }
                    if (isChargingSword)
                    {
                        Debug.Log($"[ATTACK] Ataque INTERRUMPIDO (carga no completada)");   
                        // isChargingSword = false;
                        // chargeTime = 0f;
                    }
                    else
                    {
                        Debug.Log("[ATTACK] Ataque NORMAL");
                    }

                    // damage *= 1.5f;

                    StartCoroutine(SwordAttack(damage));

                     // Resetear estados de carga
                    isChargingSword = false;
                    isFullyCharged = false;
                    chargeTime = 0f;
                }
                else if (currentlySelected == "Arco" && equippedWeapon != null)
                {
                    StartCoroutine(bowAttack());
                    Debug.Log("Ataque Arco");
                }
                else if (currentlySelected == "Ofensiva"
                && equippedAbilities.ContainsKey(AbilityType.Ofensiva)
                && offensiveAbilityCooldown == 0)
                {
                    StartCoroutine(offensiveAbility());
                    Debug.Log("Habilidad Ofensiva");
                }
                else if (currentlySelected == "Defensiva" && equippedAbilities.ContainsKey(AbilityType.Defensiva)
                && defensiveAbilityCooldown == 0)
                {
                    enableShield();
                    Debug.Log("Habilidad Defensiva");
                }
                else if (currentlySelected == "Curativa"
                && equippedAbilities.ContainsKey(AbilityType.Curativa)
                && healingAbilityCooldown == 0)
                {
                    StartCoroutine(healingAbility());
                    Debug.Log("Habilidad Curativa");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) PrintLoadout();
        if (Input.GetKeyDown(KeyCode.Alpha1) && equippedAbilities.ContainsKey(AbilityType.Ofensiva) && offensiveAbilityCooldown == 0 && currentlySelected != "Ofensiva")
        {
            currentlySelected = "Ofensiva";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && equippedAbilities.ContainsKey(AbilityType.Defensiva) && defensiveAbilityCooldown == 0 && currentlySelected != "Defensiva")
        {
            currentlySelected = "Defensiva";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && equippedAbilities.ContainsKey(AbilityType.Curativa) && healingAbilityCooldown == 0 && currentlySelected != "Curativa")
        {
            currentlySelected = "Curativa";
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
        if(defensiveAbilityHits > 0){
            defensiveAbilityHits -= 1;
            StartCoroutine(ActivarInmunidadEscudo());
            Debug.Log("The shield took a hit");
        } else {
            vidaActual -= damage;
            if (vidaActual <= 0)
            {
                vidaActual = 0;
                Die();
            }
            StartCoroutine(ActivarInmunidad());
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (weapon.weaponType == allowedWeaponType)
        {
            equippedWeapon = weapon;
            currentlySelected = weapon.weaponType.ToString();
        }
    }

    public void EquipAbility(Ability ability)
    {
        equippedAbilities.Add(ability.abilityType, ability);
    }

    public void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
        LevelManager.instance.isLevelLoaded = false;
        GameManager.instance.RestartGame();
    }

    private void comprobarEnemigosEnArea(Vector3 attackPosition, float attackRadius, float damage)
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

    private void trazarRayoArco()
    {
        PosicionCursor cursorToWorld = GetComponent<PosicionCursor>();
        Vector3 direccion = (cursorToWorld.lookPoint - transform.position).normalized;

        Ray rayo = new Ray(transform.position, direccion);
        RaycastHit hit;

        if (Physics.SphereCast(rayo, arrowRadius, out hit, bowReach))
        {
            Enemigo enemigo = hit.collider.GetComponentInParent<Enemigo>();
            Debug.DrawLine(transform.position, hit.point, Color.red, 1f); // Debugging
            if (enemigo)
            {
                enemigo.takeDamage(equippedWeapon.damage);
            }

        }
        else
        {
            Vector3 endPoint = transform.position + direccion * bowReach;
            Debug.DrawLine(transform.position, endPoint, Color.green, 1f); // Debugging
            Debug.Log("Arrow max range reached.");
        }
    }

    // Corrutina ChargeSword modificada
IEnumerator ChargeSword()
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
        Debug.Log($"[CHARGE] Progreso: {(chargeTime/chargeRequiredTime*100).ToString("F0")}%");
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

    IEnumerator SwordAttack(float damage)
{
    if (isAttacking) yield break;
    isAttacking = true;
    Vector3 attackPosition = controller.transform.position + controller.transform.forward * attackDamageOffset.z + controller.transform.up * attackDamageOffset.y + controller.transform.right * attackDamageOffset.x;
    
    // Comprobar enemigos en área y aplicar daño y empuje
    Collider[] colliders = Physics.OverlapSphere(attackPosition, attackrange);
    HashSet<Enemigo> uniqueEnemies = new HashSet<Enemigo>();

    foreach (Collider c in colliders)
    {
        Enemigo enemigo = c.GetComponentInParent<Enemigo>();
        if (enemigo != null && !uniqueEnemies.Contains(enemigo))
        {
            uniqueEnemies.Add(enemigo);
            enemigo.takeDamage(damage);
            
            // Aplicar empuje al enemigo (con comprobación de Rigidbody)
            Rigidbody enemyRb = enemigo.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                Vector3 pushDirection = (enemigo.transform.position - transform.position).normalized;
                pushDirection.y = 0; // Mantener el empuje en el plano horizontal
                enemyRb.AddForce(pushDirection * empujeFuerza, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning($"El enemigo {enemigo.name} no tiene Rigidbody, no se puede empujar");
                // Alternativa: Mover el enemigo directamente (menos realista)
                // enemigo.transform.position += pushDirection * empujeFuerza * 0.1f;
            }
        }
    }
    
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

    IEnumerator offensiveAbility()
    {
        if (!equippedAbilities.TryGetValue(AbilityType.Ofensiva, out Ability offensiveAbility))
        {
            Debug.LogWarning("No offensive ability equipped!");
            yield break;
        }

        isCastingAbility = true;
        PosicionCursor cursorToWorld = GetComponent<PosicionCursor>();
        Vector3 targetPosition = cursorToWorld.lookPoint;
        Vector3 playerPosition = transform.position;
        float distanceToTarget = Vector3.Distance(playerPosition, targetPosition);
        float maxCastRange = offensiveAbility.range;

        if (distanceToTarget > maxCastRange)
        {
            Vector3 direction = (targetPosition - playerPosition).normalized;
            targetPosition = playerPosition + direction * maxCastRange;
        }

        yield return new WaitForSeconds(0.5f);

        comprobarEnemigosEnArea(targetPosition, offensiveAbility.areaOfEffect, offensiveAbility.damage);
        offensiveAbilityCooldown = offensiveAbility.killCountCooldown;
        if(equippedWeapon) currentlySelected = equippedWeapon.weaponType.ToString();
        isCastingAbility = false;
    }

    private void enableShield(){

        if (!equippedAbilities.TryGetValue(AbilityType.Defensiva, out Ability defensiveAbility))
        {
            Debug.LogWarning("No defensive ability equipped!");
            return;
        }
        isCastingAbility = true;
        defensiveAbilityHits = 2;
        Debug.Log("Shield enabled");
        defensiveAbilityCooldown = defensiveAbility.killCountCooldown;
        isCastingAbility = false;
        StartCoroutine(shieldDuration());
        if(equippedWeapon) currentlySelected = equippedWeapon.weaponType.ToString();
    }

    IEnumerator shieldDuration(){
        yield return new WaitForSeconds(10);
        defensiveAbilityHits = 0;
        Debug.Log("Shield disabled");
    }


    IEnumerator ActivarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoImmunidad);
        inmunidad = false;
    }

    IEnumerator ActivarInmunidadEscudo()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoImmunidad);
        inmunidad = false;
    }

    IEnumerator healingAbility(){
        if (!equippedAbilities.TryGetValue(AbilityType.Curativa, out Ability healingAbility))
        {
            Debug.LogWarning("No offensive ability equipped!");
            yield break;
        }
        isCastingAbility = true;
        healingAbilityCooldown = healingAbility.killCountCooldown;
        healPlayer(60);
        isCastingAbility = false;
        if(equippedWeapon) currentlySelected = equippedWeapon.weaponType.ToString();

    }
    private void healPlayer(int ammount){
        if(vidaActual <= 0) return;
        if(vidaActual >= vidaMaxima-ammount) { vidaActual = vidaMaxima; }
        else { vidaActual += ammount; }
        
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
        Debug.Log($"Character: {GameManager.instance.currentCharacter}");
        Debug.Log($"Weapon: {(equippedWeapon != null ? equippedWeapon : "None")}");

        foreach (var ability in equippedAbilities.Values)
        {
            Debug.Log($"Ability: {ability.abilityName} ({ability.abilityType})");
        }
    }
}
