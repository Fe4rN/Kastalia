using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject DamagePopupPrefab;
    [SerializeField] private EnemyHealthbar healthbar;

    public float maxHealth = 100;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        if (EnemyManager.Instance) EnemyManager.Instance.RegisterEnemy();

        if (healthbar)
        {
            healthbar.UpdateHealthbar(maxHealth, currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (DamagePopupPrefab && currentHealth > 0) ShowDamagePopup(damage);
        if (currentHealth - damage > 0)
        {
            currentHealth -= damage;
            healthbar.gameObject.SetActive(true);
            if (healthbar)
            {
                healthbar.UpdateHealthbar(maxHealth, currentHealth);
            }
            //StartCoroutine(FlashOnHit());
        }
        else
        {
            currentHealth = 0;
            Die();
        }
    }

    private void ShowDamagePopup(int damage)
    {
        GameObject popup = Instantiate(DamagePopupPrefab, transform.position, Quaternion.identity, transform);
        popup.GetComponent<TextMeshPro>().text = damage.ToString();
    }

    public void SetHealth(int value)
    {
        if (value <= 0) return;
        maxHealth = value; currentHealth = value;
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
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if(!jugador) return; 
        OffensiveAbility offensiveAbilityController = jugador.GetComponent<OffensiveAbility>();
        DefensiveAbility defensiveAbilityController = jugador.GetComponent<DefensiveAbility>();
        HealingAbility healingAbilityController = jugador.GetComponent<HealingAbility>();

        if (offensiveAbilityController.offensiveAbilityCooldown > 0)
            offensiveAbilityController.offensiveAbilityCooldown -= 1;
        if (defensiveAbilityController.defensiveAbilityCooldown > 0)
            defensiveAbilityController.defensiveAbilityCooldown -= 1;
        if (healingAbilityController.healingAbilityCooldown > 0)
            healingAbilityController.healingAbilityCooldown -= 1;

        MainInterface mainInterface = FindFirstObjectByType<MainInterface>();
        if (mainInterface)
        {
            mainInterface.SubtractCooldown();
        }
        StopAllCoroutines();

        if (EnemyManager.Instance) EnemyManager.Instance.UnregisterEnemy();

        Destroy(gameObject);
    }
}