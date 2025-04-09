using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class HealingAbility : MonoBehaviour
{
    PlayerController playerController;
    PlayerInventory playerInventory;
    PlayerHealth playerHealth;
    private int healingAbilityCooldown = 0;


    public IEnumerator healingAbility()
    {
        if (!playerInventory.equippedAbilities.TryGetValue(AbilityType.Curativa, out Ability healingAbility))
        {
            Debug.LogWarning("No offensive ability equipped!");
            yield break;
        }
        playerController.isCastingAbility = true;
        healingAbilityCooldown = healingAbility.killCountCooldown;
        playerHealth.healPlayer(60);
        playerController.isCastingAbility = false;
        //if (equippedWeapon) currentlySelected = equippedWeapon.weaponType.ToString();
    }

}
