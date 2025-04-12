using System.Collections;
using UnityEngine;

public class OffensiveAbility : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private PlayerController playerController;
    private PosicionCursor posicionCursor;

    public int offensiveAbilityCooldown = 0;  // Cooldown en segundos
    public Sprite icon;  // Sprite para el icono de la habilidad

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerController = GetComponent<PlayerController>();
        posicionCursor = GetComponent<PosicionCursor>();
    }

    public IEnumerator ActivateOffensiveAbility()
    {
        if (offensiveAbilityCooldown > 0)
        {
            Debug.Log("Habilidad en cooldown");
            yield break;  // No ejecutar si est� en cooldown
        }

        Vector3 targetPosition = posicionCursor.lookPoint;
        Vector3 playerPosition = transform.position;
        float distanceToTarget = Vector3.Distance(playerPosition, targetPosition);
        float maxCastRange = 10f;  // Rango m�ximo de la habilidad ofensiva

        // Si la distancia al objetivo es mayor que el rango m�ximo, ajustamos la posici�n
        if (distanceToTarget > maxCastRange)
        {
            Vector3 direction = (targetPosition - playerPosition).normalized;
            targetPosition = playerPosition + direction * maxCastRange;
        }

        yield return new WaitForSeconds(0.5f);  // Tiempo de lanzamiento de la habilidad

        // Aqu� llamar�as a tu l�gica de �rea de efecto o da�o
        playerController.comprobarEnemigosEnArea(targetPosition, 5f, 10);  // �rea de efecto y da�o de ejemplo

        // Iniciar cooldown
        offensiveAbilityCooldown = 5;  // Cooldown de 5 segundos, por ejemplo

        // Comienza el contador de cooldown
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        while (offensiveAbilityCooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            offensiveAbilityCooldown--;
        }
    }
}
