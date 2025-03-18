using UnityEngine;

public class Enemigo : Maquina
{
    [SerializeField]
    public float vidaMaxima = 100f;
    public float vidaActual;
    Transform jugador;
    [SerializeField] float distanciaAtaque = 3f;
    [SerializeField] float distanciaDeteccion = 30f;

    public NombreEstado deambularEstado;
    public NombreEstado perseguirEstado;

    public float AttackDistance { get { return distanciaAtaque;}}
    public float DetectionDistance { get { return distanciaDeteccion;}}

    public Transform Player { get { return jugador; } }

    void Start()
    {
        jugador = GameObject.FindWithTag("Player").transform;
    }
}
