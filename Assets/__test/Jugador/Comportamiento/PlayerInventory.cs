using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponType allowedWeaponType;
    public Weapon weapon;
    public ItemType selectedItemType;
    public AbilityType selectedAbilityType;
    public Dictionary<AbilityType, Ability> equippedAbilities = new Dictionary<AbilityType, Ability>();


    public void EquipWeapon(Weapon weapon)
    {
        if (weapon.weaponType == allowedWeaponType)
        {
            this.weapon = weapon;
        }
    }

    public void EquipAbility(Ability ability)
    {
        equippedAbilities.Add(ability.abilityType, ability);
    }
}
