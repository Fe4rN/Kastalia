using System.Collections;
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

    public float AttackDistance { get { return distanciaAtaque; } }
    public float DetectionDistance { get { return distanciaDeteccion; } }

    public Transform Player { get { return jugador; } }

    void Start()
    {
        jugador = GameObject.FindWithTag("Player").transform;
        vidaActual = vidaMaxima;
    }

    void Update()
    {
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
