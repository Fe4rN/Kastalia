using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float vidaMaxima = 100f;
    public float vidaActual;
    private float tiempoInmunidad = .75f;
    private bool inmunidad = false;
    public int defensiveAbilityHits = 0;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void takeDamage(float damage)
    {
        if (inmunidad) return;
        if (defensiveAbilityHits > 0)
        {
            defensiveAbilityHits--;
            StartCoroutine(ActivarInmunidad());
            return;
        }
        if (vidaActual - damage > 0)
        {
            vidaActual -= damage;
            StartCoroutine(ActivarInmunidad());
        }
        else
        {
            vidaActual = 0;
            Die();
        }
    }

    public void healPlayer(int ammount)
    {
        if (vidaActual <= 0) return;
        if (vidaActual + ammount > vidaMaxima) { vidaActual = vidaMaxima; }
        else { vidaActual += ammount; }

    }

    public void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
        LevelManager.instance.isLevelLoaded = false;
        GameManager.instance.RestartGame();
    }

    IEnumerator ActivarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoInmunidad);
        inmunidad = false;
    }
}
