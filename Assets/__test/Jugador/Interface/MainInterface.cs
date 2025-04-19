using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainInterface : MonoBehaviour
{
    [SerializeField] private TMP_Text VidaText;
    [SerializeField] private Button WeaponButton;
    [SerializeField] private Button OffensiveButton;
    [SerializeField] private Button ShieldButton;
    [SerializeField] private Button PotionButton;

    private bool isPlayerFound = false;
    private GameObject player;
    private OffensiveAbility offensiveAbilityController;
    private DefensiveAbility defensiveAbilityController;
    private HealingAbility healingAbilityController;


    public void updateVidaText(float vida)
    {
        VidaText.text = vida.ToString() + " / 100";
    }

    public void updateWeaponSlot(Weapon weapon)
    {
        if (weapon.icon)
        {
            WeaponButton.GetComponent<Image>().sprite = weapon.icon;
            return;
        }
        WeaponButton.GetComponentInChildren<TMP_Text>().text = weapon.name;
    }

    public void updateHabilitySlots(AbilityType abilityType, Ability ability)
    {
        switch (abilityType)
        {
            case AbilityType.Ofensiva:
                Debug.Log(ability.abilityName);
                OffensiveButton.GetComponentInChildren<TMP_Text>().text = ability.abilityName;
                break;
            case AbilityType.Defensiva:
                ShieldButton.GetComponentInChildren<TMP_Text>().text = ability.abilityName;
                break;
            case AbilityType.Curativa:
                PotionButton.GetComponentInChildren<TMP_Text>().text = ability.abilityName;
                break;
        }
    }

    void Update()
    {
        if (!isPlayerFound)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            isPlayerFound = true;
            return;
        }

        if(!player) return;

        offensiveAbilityController = player.GetComponent<OffensiveAbility>();
        defensiveAbilityController = player.GetComponent<DefensiveAbility>();
        healingAbilityController = player.GetComponent<HealingAbility>();

        if (offensiveAbilityController.offensiveAbilityCooldown > 0)
        {
            OffensiveButton.GetComponent<TMP_Text>().text = "Cooldown: " + offensiveAbilityController.offensiveAbilityCooldown.ToString("F1");
        }
        if(defensiveAbilityController.defensiveAbilityCooldown > 0)
        {
            ShieldButton.GetComponent<TMP_Text>().text = "Cooldown: " + defensiveAbilityController.defensiveAbilityCooldown.ToString("F1");
        }
        if (healingAbilityController.healingAbilityCooldown > 0)
        {
            PotionButton.GetComponent<TMP_Text>().text = "Cooldown: " + healingAbilityController.healingAbilityCooldown.ToString("F1");
        }
    }
}
