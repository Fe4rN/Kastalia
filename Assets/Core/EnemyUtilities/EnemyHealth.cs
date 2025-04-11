using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public GameObject jugador;

    private bool yaRegistrado = false;
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        jugador = GameObject.FindGameObjectWithTag("Player");

        if (!yaRegistrado && EnemyManager.Instance != null)
        {
            EnemyManager.Instance.RegisterEnemy();
            yaRegistrado = true;
        }
    }

    private void Update()
    {
        if (jugador) return;
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (currentHealth - damage > 0)
        {
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
        if (isDead) return; // Evita que Die() se ejecute más de una vez
        isDead = true;

        // Para futuras implementaciones
        OffensiveAbility offensiveAbilityController = jugador.GetComponent<OffensiveAbility>();
        DefensiveAbility defensiveAbilityController = jugador.GetComponent<DefensiveAbility>();
        HealingAbility healingAbilityController = jugador.GetComponent<HealingAbility>();

        if (offensiveAbilityController.offensiveAbilityCooldown > 0)
            offensiveAbilityController.offensiveAbilityCooldown -= 1;
        if (defensiveAbilityController.defensiveAbilityCooldown > 0)
            defensiveAbilityController.defensiveAbilityCooldown -= 1;
        if (healingAbilityController.healingAbilityCooldown > 0)
            healingAbilityController.healingAbilityCooldown -= 1;

        StopAllCoroutines();

        // Reemplazar la llamada directa con el bloque que maneja null
        if (EnemyManager.Instance != null)
        {
            EnemyManager.Instance.UnregisterEnemy();
        }
        else
        {
            Debug.LogWarning("EnemyManager.Instance es null. Buscando uno manualmente...");
            EnemyManager manager = FindObjectOfType<EnemyManager>();
            if (manager != null)
            {
                manager.UnregisterEnemy();
            }
            else
            {
                Debug.LogError("¡No se encontró ningún EnemyManager en la escena!");
            }
        }

        Destroy(gameObject);
    }

}