using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public GameObject jugador;

    private void Start()
    {
        currentHealth = maxHealth;
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(jugador) return;
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    {
        if(currentHealth - damage > 0){
            currentHealth -= damage;
            StartCoroutine(FlashOnHit());
        }
        else
        {
            currentHealth = 0;
            Die();
        }
    }

    IEnumerator FlashOnHit()
    {
        Renderer enemyRenderer = GetComponentInChildren<Renderer>();
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = originalColor;
    }
    private void Die()
    {
        //Para futuras implementaciones
        OffensiveAbility offensiveAbilityController = jugador.GetComponent<OffensiveAbility>();
        DefensiveAbility defensiveAbilityController = jugador.GetComponent<DefensiveAbility>();
        HealingAbility healingAbilityController = jugador.GetComponent<HealingAbility>();
        if (offensiveAbilityController.offensiveAbilityCooldown > 0) offensiveAbilityController.offensiveAbilityCooldown -= 1;
        if (defensiveAbilityController.defensiveAbilityCooldown > 0) defensiveAbilityController.defensiveAbilityCooldown -= 1;
        if (healingAbilityController.healingAbilityCooldown > 0) healingAbilityController.healingAbilityCooldown -= 1;
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
