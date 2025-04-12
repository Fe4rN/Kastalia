using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability abilityData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventario = other.GetComponent<PlayerInventory>();

        if (inventario != null && abilityData != null)
        {
            // Determina qué tipo de habilidad estamos manejando
            switch (abilityData.abilityType)
            {
                case AbilityType.Ofensiva:
                    var ofensiva = other.GetComponent<OffensiveAbility>();
                    if (ofensiva != null)
                    {
                        inventario.EquipOffensiveAbility(ofensiva);
                        Destroy(gameObject);
                    }
                    break;

                case AbilityType.Defensiva:
                    var defensiva = other.GetComponent<DefensiveAbility>();
                    if (defensiva != null)
                    {
                        inventario.EquipDefensiveAbility(defensiva);
                        Destroy(gameObject);
                    }
                    break;

                case AbilityType.Curativa:
                    var curativa = other.GetComponent<HealingAbility>();
                    if (curativa != null)
                    {
                        inventario.EquipHealingAbility(curativa);
                        Destroy(gameObject);
                    }
                    break;

                default:
                    Debug.LogWarning("Tipo de habilidad no reconocido en AbilityPickup.");
                    break;
            }
        }
    }
}
