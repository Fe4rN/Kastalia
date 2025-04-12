using UnityEngine;

public class PetFollow : MonoBehaviour
{
    public float followDistance = 5f;
    public float heightOffset = 1f;      // Altura a la que volará con respecto al jugador
    public float moveSpeed = 4f;
    public float rotationSpeed = 2f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculamos la posición objetivo de la mascota
        Vector3 offset = -player.forward * followDistance + Vector3.up * heightOffset;
        Vector3 targetPosition = player.position + offset;

        // Movimiento suave hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        // Rotación suave hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
