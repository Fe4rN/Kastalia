using System.Collections;
using UnityEngine;

public class DefensiveAbility : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerInventory playerInventory;
    private PlayerHealth playerHealth;

    public int defensiveAbilityCooldown = 0;  // Cooldown en segundos
    public Sprite icon;  // Sprite para el icono de la habilidad

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void EnableShield()
    {
        if (defensiveAbilityCooldown > 0)
        {
            Debug.Log("Habilidad en cooldown");
            return;  // No ejecutar si está en cooldown
        }

        playerController.isCastingAbility = true;
        playerHealth.defensiveAbilityHits = 2;  // El escudo puede bloquear dos hits
        Debug.Log("Escudo activado");

        // Iniciar el cooldown
        defensiveAbilityCooldown = 5;  // Cooldown de 5 segundos

        // Comienza el contador de cooldown
        StartCoroutine(Cooldown());

        // Llamar a la duración del escudo (10 segundos de protección)
        StartCoroutine(ShieldDuration());

        playerController.isCastingAbility = false;
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(10);  // El escudo dura 10 segundos
        playerHealth.defensiveAbilityHits = 0;  // Deshabilitar el escudo

        Debug.Log("Escudo desactivado");
        playerInventory.selectedItemType = ItemType.Arma;  // Volver a seleccionar el arma
    }

    private IEnumerator Cooldown()
    {
        while (defensiveAbilityCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            defensiveAbilityCooldown--;  // Reducir cooldown por segundo
        }
    }
}
