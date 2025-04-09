using System.Collections;
using UnityEngine;

public class OffensiveAbility : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private PlayerController playerController;
    private PosicionCursor posicionCursor;
    public int offensiveAbilityCooldown = 0;

    void Start(){
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        posicionCursor = GetComponent<PosicionCursor>();
    }

    public IEnumerator offensiveAbility()
    {
        if (!playerInventory.equippedAbilities.TryGetValue(AbilityType.Ofensiva, out Ability offensiveAbility))
        {
            Debug.LogWarning("No offensive ability equipped!");
            yield break;
        }

        Vector3 targetPosition = posicionCursor.lookPoint;
        Vector3 playerPosition = transform.position;
        float distanceToTarget = Vector3.Distance(playerPosition, targetPosition);
        float maxCastRange = offensiveAbility.range;

        if (distanceToTarget > maxCastRange)
        {
            Vector3 direction = (targetPosition - playerPosition).normalized;
            targetPosition = playerPosition + direction * maxCastRange;
        }

        yield return new WaitForSeconds(0.5f);

        playerController.comprobarEnemigosEnArea(targetPosition, offensiveAbility.areaOfEffect, offensiveAbility.damage);
        offensiveAbilityCooldown = offensiveAbility.killCountCooldown;
        // if (playerInventory.equippedWeapon) currentlySelected = equippedWeapon.weaponType.ToString();
    }
}
