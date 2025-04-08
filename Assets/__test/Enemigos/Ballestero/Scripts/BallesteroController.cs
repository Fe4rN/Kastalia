using UnityEngine;

public class BallesteroController : Maquina
{
    //Atributos relacionados al enemigo
    [SerializeField] private float vidaMaxima;
    private float vidaActual;
    public float shootingDistance;
    public float safeDistance;

    //Atributos relacionados al jugador
    public Transform jugador;
    public float distanciaAJugador;
    

    //Estados
    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;


    void Start()
    {
        if (FindFirstObjectByType<CharacterController>() == null) { return; }
        else { jugador = FindFirstObjectByType<CharacterController>().transform; }
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if (!jugador)
        {
            if (FindFirstObjectByType<CharacterController>() == null)
            {
                return;
            }
            else
            {
                jugador = FindFirstObjectByType<CharacterController>().transform;
            }
            distanciaAJugador = getDistanceToPlayer();
        }
    }

    private void Die()
    {
        
    }

    public void takeDamage(float damage)
    {
        if (vidaActual - damage > 0){
            vidaActual -= damage;
        } else {
            vidaActual = 0;
            Die();
        }
    }

    private float getDistanceToPlayer(){
        if (jugador){
            return Vector3.Distance(transform.position, jugador.position);
        } else {
            Debug.LogError("No se ha encontrado al jugador.");
            return 0;
        }
    }
}
