using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Para usar Image

public class PlayerInventory : MonoBehaviour
{
    public WeaponType allowedWeaponType;
    public Weapon weapon;
    public ItemType selectedItemType;
    public AbilityType selectedAbilityType;

    // Habilidades específicas
    public OffensiveAbility offensiveAbility;
    public DefensiveAbility defensiveAbility;
    public HealingAbility healingAbility;

    // SLOT UI: slot1 para arma, slot2 ofensiva, slot3 defensiva, slot4 curación
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;

    public void EquipWeapon(Weapon newWeapon)
    {
        if (newWeapon.weaponType == allowedWeaponType)
        {
            weapon = newWeapon;

            // Mostrar el sprite en el slot del inventario
            if (slot1 != null && weapon.icon != null)
            {
                slot1.sprite = weapon.icon;
                slot1.enabled = true;
            }
        }
    }

    public void EquipOffensiveAbility(OffensiveAbility ability)
    {
        offensiveAbility = ability;

        // Mostrar el sprite de la habilidad ofensiva en el slot correspondiente
        if (slot2 != null && ability.icon != null)
        {
            slot2.sprite = ability.icon;
            slot2.enabled = true;
        }
    }

    public void EquipDefensiveAbility(DefensiveAbility ability)
    {
        defensiveAbility = ability;

        // Mostrar el sprite de la habilidad defensiva en el slot correspondiente
        if (slot3 != null && ability.icon != null)
        {
            slot3.sprite = ability.icon;
            slot3.enabled = true;
        }
    }

    public void EquipHealingAbility(HealingAbility ability)
    {
        healingAbility = ability;

        // Mostrar el sprite de la habilidad de curación en el slot correspondiente
        if (slot4 != null && ability.icon != null)
        {
            slot4.sprite = ability.icon;
            slot4.enabled = true;
        }
    }
}
