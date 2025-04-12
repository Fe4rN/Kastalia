using UnityEngine;
using System.Collections.Generic;

public class EnemyLookAtSystem : MonoBehaviour
{
    public float detectionRange = 15f;
    public LayerMask enemyMask;
    private Transform currentTarget;
    private Transform player;
    private bool isLocked = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }
    }

    void Update()
    {
        if (GameManager.instance.isPaused || player == null) return;

        // Desbloquear enemigo con tecla º
        if (Input.GetKeyDown(KeyCode.BackQuote)) // Tecla º
        {
            isLocked = false;
            currentTarget = null;
        }

        // Hacer lock a un enemigo con click izquierdo si lo apuntas
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Transform aimedEnemy = GetEnemyUnderMouse();
            if (aimedEnemy != null)
            {
                currentTarget = aimedEnemy;
                isLocked = true;
            }
        }

        // Girar hacia el objetivo si está fijado
        if (currentTarget != null && isLocked)
        {
            Vector3 targetPos = new Vector3(currentTarget.position.x, player.position.y, currentTarget.position.z);
            player.LookAt(targetPos);

            // Desbloquear si sale del rango
            if (Vector3.Distance(player.position, currentTarget.position) > detectionRange)
            {
                isLocked = false;
                currentTarget = null;
            }
        }
    }

    Transform GetEnemyUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, enemyMask))
        {
            float distance = Vector3.Distance(player.position, hit.transform.position);
            if (distance <= detectionRange)
            {
                return hit.transform;
            }
        }

        return null;
    }
}
