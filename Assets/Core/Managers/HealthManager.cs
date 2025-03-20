using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public TMP_Text healthText;
    private GameObject player;
    private float maxHealth;
    private float currentHealth;

    void Start()
    {
        if(player){
            player = GameObject.FindWithTag("Player");
            maxHealth = player.GetComponent<Acciones>().vidaMaxima;  
        }
        
    }
    void Update()
    {
        if(player == null) return;
        if(player.GetComponent<Acciones>().vidaActual == 0) {
            healthText.text = "Vida: 0/" + maxHealth;
            return;
        };
        currentHealth = player.GetComponent<Acciones>().vidaActual;
        healthText.text = "Vida: " + currentHealth + "/" + maxHealth;
    }
}
