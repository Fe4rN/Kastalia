using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public WeaponType allowedWeaponType;
    public Weapon weapon;
    public ItemType selectedItemType;
    public AbilityType selectedAbilityType;
    public Dictionary<AbilityType, Ability> equippedAbilities = new Dictionary<AbilityType, Ability>();

    PlayerController controller;
    MainInterface hud;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        hud = FindFirstObjectByType<MainInterface>();
    }


    public void EquipWeapon(Weapon weapon)
    {
        if (weapon.weaponType == allowedWeaponType)
        {
            this.weapon = weapon;
            hud.EnablePickUpSlot(ItemType.Arma, AbilityType.None);
            hud.DispararNotificacion(weapon.name);
        }
    }

    public bool HasWeapon(Weapon other)
    {
        return weapon != null && weapon.name == other.name;
    }

    public void EquipAbility(Ability ability)
    {
        if (!equippedAbilities.ContainsKey(ability.abilityType))
        {
            equippedAbilities.Add(ability.abilityType, ability);
            hud.EnablePickUpSlot(ItemType.Habilidad, ability.abilityType);
        }
    }

    public bool HasAbility(Ability ability)
    {
        return equippedAbilities.ContainsKey(ability.abilityType);
    }
}
