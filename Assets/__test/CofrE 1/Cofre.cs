using UnityEngine;

public class Cofre : MonoBehaviour
{
    public GameObject[] objetosPosibles;
    public Transform puntoDeSpawn;
    public float distanciaDeInteraccion = 7f;

    private bool abierto = false;
    private Transform jugador;

    void Start()
    {
        GameObject objJugador = GameObject.FindGameObjectWithTag("Player");
        if (objJugador != null)
        {
            jugador = objJugador.transform;
            Debug.Log("Jugador encontrado: " + jugador.name);
        }
        else
        {
            Debug.LogError("¡No se encontró un objeto con tag 'Player'!");
        }
    }

    void Update()
    {
        if (abierto || jugador == null) return;

        
        Vector3 posCofreXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 posJugadorXZ = new Vector3(jugador.position.x, 0, jugador.position.z);
        float distancia = Vector3.Distance(posCofreXZ, posJugadorXZ);

        if (distancia <= distanciaDeInteraccion)
        {
            Debug.Log("¡Dentro del rango!");
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("¡Tecla F presionada!");
                AbrirCofre();
            }
        }
    }

    void AbrirCofre()
    {
        abierto = true;

        int indiceAleatorio = Random.Range(0, objetosPosibles.Length);
        GameObject objeto = Instantiate(
            objetosPosibles[indiceAleatorio],
            puntoDeSpawn.position,
            Quaternion.identity
        );

       
        objeto.transform.localScale = Vector3.one;

        Debug.Log("¡Cofre abierto con la letra F!");
    }
}
