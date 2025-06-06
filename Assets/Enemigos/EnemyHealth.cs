using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

    private void Die()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if(!jugador) return; 
        PlayerInventory playerInventory = jugador.GetComponent<PlayerInventory>();
        OffensiveAbility offensiveAbilityController = jugador.GetComponent<OffensiveAbility>();
        DefensiveAbility defensiveAbilityController = jugador.GetComponent<DefensiveAbility>();
        HealingAbility healingAbilityController = jugador.GetComponent<HealingAbility>();

        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva) && offensiveAbilityController.offensiveAbilityCooldown > 0)
            offensiveAbilityController.offensiveAbilityCooldown -= 1;
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva) && defensiveAbilityController.defensiveAbilityCooldown > 0)
            defensiveAbilityController.defensiveAbilityCooldown -= 1;
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa) && healingAbilityController.healingAbilityCooldown > 0)
            healingAbilityController.healingAbilityCooldown -= 1;

        MainInterface mainInterface = FindFirstObjectByType<MainInterface>();
        if (mainInterface)
        {
            mainInterface.SetCooldowns();
        }
        StopAllCoroutines();

        if (EnemyManager.Instance) EnemyManager.Instance.UnregisterEnemy();

        Destroy(gameObject);
    }
}