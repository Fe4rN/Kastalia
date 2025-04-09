using System.Collections;
using UnityEngine;

public class DefensiveAbility : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerInventory playerInventory;
    public PlayerHealth playerHealth;
    public int defensiveAbilityCooldown = 0;

    public void enableShield()
    {

        if (!playerInventory.equippedAbilities.TryGetValue(AbilityType.Defensiva, out Ability defensiveAbility))
        {
            Debug.LogWarning("No defensive ability equipped!");
            return;
        }
        playerController.isCastingAbility = true;
        playerHealth.defensiveAbilityHits = 2;
        Debug.Log("Shield enabled");
        defensiveAbilityCooldown = defensiveAbility.killCountCooldown;
        playerController.isCastingAbility = false;
        StartCoroutine(shieldDuration());
        //if (equippedWeapon) currentlySelected = equippedWeapon.weaponType.ToString();
    }

    public IEnumerator shieldDuration()
    {
        yield return new WaitForSeconds(10);
        playerHealth.defensiveAbilityHits = 0;
        Debug.Log("Shield disabled");
    }
}
