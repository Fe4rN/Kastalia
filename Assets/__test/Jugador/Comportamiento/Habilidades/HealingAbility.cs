using System.Collections;
using UnityEngine;

public class HealingAbility : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerInventory playerInventory;
    private PlayerHealth playerHealth;

    public int healingAbilityCooldown = 0;  // Cooldown en segundos
    public Sprite icon;  // Sprite para el icono de la habilidad

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public IEnumerator ActivateHealingAbility()
    {
        if (healingAbilityCooldown > 0)
        {
            Debug.Log("Habilidad en cooldown");
            yield break;  // No ejecutar si está en cooldown
        }

        playerController.isCastingAbility = true;
        Debug.Log("Curación activada");

        // Curar al jugador
        playerHealth.HealPlayer(60);  // Curación de 60 HP, puedes cambiar esto

        // Iniciar el cooldown
        healingAbilityCooldown = 5;  // Cooldown de 5 segundos

        // Comienza el contador de cooldown
        StartCoroutine(Cooldown());

        playerController.isCastingAbility = false;
    }

    private IEnumerator Cooldown()
    {
        while (healingAbilityCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            healingAbilityCooldown--;  // Reducir cooldown por segundo
        }
    }
}
