using UnityEngine;

public class ObstaculoTransparente : MonoBehaviour
{
    [SerializeField] UnityEngine.Material transparencia;
    [SerializeField] UnityEngine.Material opaco;

    Renderer rend;
    Collider col;

    public bool hitted = false; // Esta es la propiedad que te falta

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        if (opaco == null)
        {
            opaco = rend.material;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cambiar el material a transparente
            rend.material = transparencia;

            // Marcar que la pared fue tocada
            hitted = true;

            // Intentamos obtener el componente PlayerMovement
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.BlockPlayer(true);  // Bloquear el movimiento del jugador
            }
            else
            {
                Debug.LogError("No se encontró el componente PlayerMovement en el jugador.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Restaurar el material original de la pared
            rend.material = opaco;

            // Marcar que la pared dejó de ser tocada
            hitted = false;

            // Intentamos obtener el componente PlayerMovement
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.BlockPlayer(false);  // Desbloquear el movimiento del jugador
            }
            else
            {
                Debug.LogError("No se encontró el componente PlayerMovement en el jugador.");
            }
        }
    }
}
