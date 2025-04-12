using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public int vidaMaxima = 10;
    public int vidaActual;

    [Header("Inmunidad y defensas")]
    private float tiempoInmunidad = 0.75f;
    private bool inmunidad = false;
    public int defensiveAbilityHits = 0;

    [Header("Eventos")]
    public UnityEvent<int> cambioVida;

    void Start()
    {
        vidaActual = vidaMaxima;
        cambioVida?.Invoke(vidaActual);
    }

    public void TakeDamage(int damage)
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

        cambioVida?.Invoke(vidaActual);
    }

    public void HealPlayer(int amount)
    {
        if (vidaActual <= 0) return;

        vidaActual = Mathf.Min(vidaActual + amount, vidaMaxima);
        cambioVida?.Invoke(vidaActual);
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
