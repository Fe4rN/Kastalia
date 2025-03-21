using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability abilityData;

    private void OnTriggerEnter(Collider other)
    {
        Acciones jugador = other.GetComponent<Acciones>();

        if(jugador){
            jugador.EquipAbility(abilityData);
            if (jugador.equippedAbilities.ContainsKey(abilityData.abilityType)){
                Destroy(gameObject);
            }
        }
    }
}
