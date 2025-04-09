using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability abilityData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventario = other.GetComponent<PlayerInventory>();

        if(inventario){
            inventario.EquipAbility(abilityData);
            if (inventario.equippedAbilities.ContainsKey(abilityData.abilityType)){
                Destroy(gameObject);
            }
        }
    }
}
