using System.Collections;
using UnityEngine;

public class Enemigo : Maquina
{
    [SerializeField] public float vidaMaxima = 50f;
    public float vidaActual;
    Transform jugador;
    public float distanciaAtaque = 3f;
    [SerializeField] float distanciaDeteccion = 5f;
    float distanceToPlayer;

    public float attackDamage = 10f;

    public NombreEstado deambularEstado;
    public NombreEstado perseguirEstado;
    public NombreEstado atacarEstado;

    public float AttackDistance { get { return distanciaAtaque; } }
    public float DetectionDistance { get { return distanciaDeteccion; } }

    public Transform Player { get { return jugador; } }

    void Start()
    {
        if (GameObject.FindWithTag("Player") == null) return;
        jugador = GameObject.FindWithTag("Player").transform;
        vidaActual = vidaMaxima;

        if (deambularEstado != null)
        {
            SetEstado(deambularEstado.Value); // Activa estado deambular
        }

    }

    void Update()
    {
        if (GameObject.FindWithTag("Player") == null)
        {
            return;
        }
        else
        {
            jugador = GameObject.FindWithTag("Player").transform;
        }
        if (vidaActual <= 0) Die();
    }

    public void takeDamage(float damage)
    {
        if (vidaActual <= 0) return;
        StartCoroutine(FlashOnHit());
        vidaActual -= damage;
        Debug.Log($"Ouch! -{damage}");
    }
    IEnumerator FlashOnHit()
    {
        Renderer enemyRenderer = GetComponentInChildren<Renderer>();
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = originalColor;
    }
    private void Die()
    {
        //Para futuras implementaciones
        Acciones statsJugador = jugador.GetComponent<Acciones>();
        if (statsJugador.offensiveAbilityCooldown > 0) statsJugador.offensiveAbilityCooldown -= 1;
        if (statsJugador.defensiveAbilityCooldown > 0) statsJugador.defensiveAbilityCooldown -= 1;
        if (statsJugador.healingAbilityCooldown > 0) statsJugador.healingAbilityCooldown -= 1;
        StopAllCoroutines();
        Destroy(gameObject);
    }
}

public enum EnemigoTipo { Melee, Distancia }
