using UnityEngine;

public class MainInterface : MonoBehaviour
{
    [SerializeField] Slot WeaponSlot;
    [SerializeField] Slot OffensiveAbilitySlot;
    [SerializeField] Slot ShieldAbilitySlot;
    [SerializeField] Slot PotionAbilitySlot;

    [SerializeField] Notificacion notificacion;

    private GameObject player;
    private PlayerInventory playerInventory;
    private OffensiveAbility offensiveAbilityController;
    private DefensiveAbility defensiveAbilityController;
    private HealingAbility healingAbilityController;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
            offensiveAbilityController = player.GetComponent<OffensiveAbility>();
            defensiveAbilityController = player.GetComponent<DefensiveAbility>();
            healingAbilityController = player.GetComponent<HealingAbility>();
        }

        // WeaponSlot.Disable();
        // OffensiveAbilitySlot.Disable();
        // ShieldAbilitySlot.Disable();
        // PotionAbilitySlot.Disable();
    }

    public void SelectSlot(ItemType itemType, AbilityType abilityType)
    {
        if (itemType == ItemType.Arma)
        {
            WeaponSlot.HighlightSlot(true);
            OffensiveAbilitySlot.HighlightSlot(false);
            ShieldAbilitySlot.HighlightSlot(false);
            PotionAbilitySlot.HighlightSlot(false);
        }
        else if (abilityType == AbilityType.Ofensiva)
        {
            WeaponSlot.HighlightSlot(false);
            OffensiveAbilitySlot.HighlightSlot(true);
            ShieldAbilitySlot.HighlightSlot(false);
            PotionAbilitySlot.HighlightSlot(false);
        }
        else if (abilityType == AbilityType.Defensiva)
        {
            WeaponSlot.HighlightSlot(false);
            OffensiveAbilitySlot.HighlightSlot(false);
            ShieldAbilitySlot.HighlightSlot(true);
            PotionAbilitySlot.HighlightSlot(false);
        }
        else if (abilityType == AbilityType.Curativa)
        {
            WeaponSlot.HighlightSlot(false);
            OffensiveAbilitySlot.HighlightSlot(false);
            ShieldAbilitySlot.HighlightSlot(false);
            PotionAbilitySlot.HighlightSlot(true);
        }
    }

    public void EnablePickUpSlot(ItemType itemType, AbilityType abilityType)
    {
        if (itemType == ItemType.Arma)
        {
            WeaponSlot.Enable();
        }
        else if (abilityType == AbilityType.Ofensiva)
        {
            OffensiveAbilitySlot.Enable();
        }
        else if (abilityType == AbilityType.Defensiva)
        {
            ShieldAbilitySlot.Enable();
        }
        else if (abilityType == AbilityType.Curativa)
        {
            PotionAbilitySlot.Enable();
        }
    }

    public void SetCooldowns()
    {
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Ofensiva)) OffensiveAbilitySlot.setCooldown(offensiveAbilityController.offensiveAbilityCooldown);
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Defensiva)) ShieldAbilitySlot.setCooldown(defensiveAbilityController.defensiveAbilityCooldown);
        if (playerInventory.equippedAbilities.ContainsKey(AbilityType.Curativa)) PotionAbilitySlot.setCooldown(healingAbilityController.healingAbilityCooldown);
    }

    public void DispararNotificacion(string nombreObjeto)
    {
        if (notificacion)
        {
            notificacion.EstablecerNombreObjeto(nombreObjeto);
            notificacion.gameObject.SetActive(true);
        }
    }
}
