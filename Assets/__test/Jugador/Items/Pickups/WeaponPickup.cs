using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponData;

    private void OnTriggerEnter(Collider other)
    {
        Acciones jugador = other.GetComponent<Acciones>();

        if(jugador){
            jugador.EquipWeapon(weaponData);
            if (jugador.equippedWeapon != null && jugador.equippedWeapon == weaponData){
                Destroy(gameObject);
            }
        }
    }
}
