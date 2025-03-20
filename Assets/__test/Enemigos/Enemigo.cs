using System.Collections;
using UnityEngine;

public class Enemigo : Maquina
{
    [SerializeField] public float vidaMaxima = 50f;
    public float vidaActual;
    Transform jugador;
    [SerializeField] float distanciaAtaque = 3f;
    [SerializeField] float distanciaDeteccion = 30f;
    float distanceToPlayer;

    private float attackCooldown = 1.5f;
    public float attackDamage = 10f;

    public NombreEstado deambularEstado;
    public NombreEstado perseguirEstado;

    public float AttackDistance { get { return distanciaAtaque; } }
    public float DetectionDistance { get { return distanciaDeteccion; } }

    public Transform Player { get { return jugador; } }

    void Start()
    {   if(GameObject.FindWithTag("Player") == null) return;
        jugador = GameObject.FindWithTag("Player").transform;
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if (GameObject.FindWithTag("Player") == null) {
            return; 
        }
         else {
            jugador = GameObject.FindWithTag("Player").transform;
        }
        if (vidaActual <= 0) Die();
    }

    public void takeDamage(float damage)
    {
        if(vidaActual <= 0) return;
        StartCoroutine(FlashOnHit());
        vidaActual -= damage;
    }
    IEnumerator FlashOnHit()
    {
        Renderer enemyRenderer = GetComponentInChildren<Renderer>();
        Color originalColor = enemyRenderer.material.color;
        enemyRenderer.material.color = Color.blue;
        yield return new WaitForSeconds(0.2f);
        enemyRenderer.material.color = originalColor;
    }
    private void Die(){
        //Para futuras implementaciones
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
