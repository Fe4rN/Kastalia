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
        weaponButtonText.text = jugador.equippedWeapon.weaponName;
    }
}
