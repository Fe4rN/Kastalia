using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private GameObject jugador;

    private void Start()
    {
        currentHealth = maxHealth;
        jugador = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0) currentHealth -= damage;
        else
        {
            currentHealth = 0;
            Die();
        }
        StartCoroutine(FlashOnHit());
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
