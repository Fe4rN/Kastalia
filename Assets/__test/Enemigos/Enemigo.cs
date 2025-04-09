using System.Collections;
using UnityEngine;

public class Enemigo : Maquina
{
    [SerializeField] public float vidaMaxima = 50f;
    public float vidaActual;
    Transform jugador;
    [SerializeField] float distanciaAtaque = 3f;
    [SerializeField] float distanciaDeteccion = 5f;
    float distanceToPlayer;

    public float attackDamage = 10f;

    public NombreEstado deambularEstado;
    public NombreEstado perseguirEstado;

    public float AttackDistance { get { return distanciaAtaque; } }
    public float DetectionDistance { get { return distanciaDeteccion; } }

    public Transform Player { get { return jugador; } }

    void Start()
    {   if(GameObject.FindWithTag("Player") == null) return;
        jugador = GameObject.FindWithTag("Player").transform;
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if (GameObject.FindWithTag("Player") == null) {
            return; 
        }
         else {
            jugador = GameObject.FindWithTag("Player").transform;
        }
       if (vidaActual <= 0) Die();
    }

    public void takeDamage(float damage)
    {
        if(vidaActual <= 0) return;
        StartCoroutine(FlashOnHit());
        vidaActual -= damage;
        Debug.Log($"Ouch! -{damage}");
    }
    IEnumerator FlashOnHit()
    {
        Renderer enemyRenderer = GetComponentInChildren<Renderer>();
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = originalColor;
    }
    private void Die(){
        //Para futuras implementaciones
        OffensiveAbility offensiveAbilityController = jugador.GetComponent<OffensiveAbility>();
        DefensiveAbility defensiveAbilityController = jugador.GetComponent<DefensiveAbility>();
        HealingAbility healingAbilityController = jugador.GetComponent<HealingAbility>();
        if(offensiveAbilityController.offensiveAbilityCooldown > 0) offensiveAbilityController.offensiveAbilityCooldown -= 1;
        if(defensiveAbilityController.defensiveAbilityCooldown > 0) defensiveAbilityController.defensiveAbilityCooldown -= 1;
        if(healingAbilityController.healingAbilityCooldown > 0) healingAbilityController.healingAbilityCooldown -= 1;
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
