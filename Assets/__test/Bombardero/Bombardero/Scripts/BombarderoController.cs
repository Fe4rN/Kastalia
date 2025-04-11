using UnityEngine;

public class BombarderoController : Maquina
{
    //Atributos relacionados al enemigo
    public float shootingDistance = 15f;
    public float safeDistance = 10f;
    public float fireCooldown = 10f;

    //Atributos relacionados al jugador
    public Transform jugador;
    public float distanciaAJugador;
    
    //Estados
    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;
    public GameObject bombaPrefab;

    void Start()
    {
        if (FindFirstObjectByType<CharacterController>() == null) { return; }
        else { jugador = FindFirstObjectByType<CharacterController>().transform; }
    }

    void Update()
    {
        if (!jugador)
        {
            if (FindFirstObjectByType<CharacterController>() == null) return;
            else{ jugador = FindFirstObjectByType<CharacterController>().transform; }
        }
        distanciaAJugador = getDistanceToPlayer();
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
