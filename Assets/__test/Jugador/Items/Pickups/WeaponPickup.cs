using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventario = other.GetComponent<PlayerInventory>();

        if(inventario){
            inventario.EquipWeapon(weaponData);
            if (inventario.weapon == weaponData){
                Destroy(gameObject);
            }
        }
    }
}
