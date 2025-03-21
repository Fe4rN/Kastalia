using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text weaponButtonText;
    public Button weaponButton;
    public TMP_Text offensiveAbilityButtonText;
    public Button offensiveAbilityButton;
    public TMP_Text defensiveAbilityButtonText;
    public Button defensiveAbilityButton;
    public TMP_Text healingAbilityButtonText;
    public Button healingAbilityButton;
    private GameObject player;
    private float maxHealth;
    private float currentHealth;
    private bool uiShown = false;

    private Color defaultButtonColor;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        defaultButtonColor = weaponButton.GetComponent<Image>().color;
    }

    void Update()
    {
        if (GameManager.instance.playerSpawned && uiShown != true) startUI();
        if (GameManager.instance.isPaused) return;
        if (player == null) return;
        Acciones jugador = player.GetComponent<Acciones>();
        if (jugador.vidaActual == 0)
        {
            healthText.text = "DEAD";
            return;
        }
        ;
        currentHealth = player.GetComponent<Acciones>().vidaActual;
        healthText.text = "Vida: " + currentHealth + "/" + maxHealth;

        updateWeapon();
        updateAbilities();
    }

    private void startUI()
    {
        healthText = GameObject.Find("Health").GetComponent<TMP_Text>();
        weaponButton = GameObject.Find("Weapon").GetComponent<Button>();
        weaponButtonText = weaponButton.GetComponentInChildren<TMP_Text>();
        player = GameObject.FindWithTag("Player");
        maxHealth = player.GetComponent<Acciones>().vidaMaxima;
        uiShown = true;
    }

    private void updateWeapon()
    {
        if (player == null) return;
        Acciones jugador = player.GetComponent<Acciones>();
        if (jugador == null || jugador.equippedWeapon == null) return;

        // Update weapon button text
        weaponButtonText.text = jugador.equippedWeapon.weaponName;

        // Highlight selected weapon button
        if (jugador.currentlySelected == jugador.equippedWeapon.weaponType.ToString())
        {
            weaponButton.GetComponent<Image>().color = highlightColor; // Highlight color
        }
        else
        {
            weaponButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
        }
    }

    private void updateAbilities()
    {
        if (player == null) return;
        Acciones jugador = player.GetComponent<Acciones>();
        if (jugador == null) return;

        // Update Offensive Ability Button Text
        if (jugador.equippedAbilities.TryGetValue(AbilityType.Ofensiva, out Ability offensiveAbility))
        {
            offensiveAbilityButtonText.text = offensiveAbility != null ? offensiveAbility.abilityName : "None";

            // Highlight selected offensive ability button
            if (jugador.currentlySelected == offensiveAbility.abilityType.ToString())
            {
                offensiveAbilityButton.GetComponent<Image>().color = highlightColor; // Highlight color
            }
            else
            {
                offensiveAbilityButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
            }
        }
        else
        {
            offensiveAbilityButtonText.text = "None";
            offensiveAbilityButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
        }

        // Update Defensive Ability Button Text
        if (jugador.equippedAbilities.TryGetValue(AbilityType.Defensiva, out Ability defensiveAbility))
        {
            defensiveAbilityButtonText.text = defensiveAbility != null ? defensiveAbility.abilityName : "None";

            // Highlight selected defensive ability button
            if (jugador.currentlySelected == defensiveAbility.abilityType.ToString())
            {
                defensiveAbilityButton.GetComponent<Image>().color = highlightColor; // Highlight color
            }
            else
            {
                defensiveAbilityButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
            }
        }
        else
        {
            defensiveAbilityButtonText.text = "None";
            defensiveAbilityButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
        }

        // Update Healing Ability Button Text
        if (jugador.equippedAbilities.TryGetValue(AbilityType.Curativa, out Ability healingAbility))
        {
            healingAbilityButtonText.text = healingAbility != null ? healingAbility.abilityName : "None";

            // Highlight selected healing ability button
            if (jugador.currentlySelected == healingAbility.abilityType.ToString())
            {
                healingAbilityButton.GetComponent<Image>().color = highlightColor; // Highlight color
            }
            else
            {
                healingAbilityButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
            }
        }
        else
        {
            healingAbilityButtonText.text = "None";
            healingAbilityButton.GetComponent<Image>().color = defaultButtonColor; // Reset to default color
        }
    }
}
