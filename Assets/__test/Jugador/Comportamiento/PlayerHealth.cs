using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float vidaMaxima = 100f;
    public float vidaActual;
    private float tiempoInmunidad = .75f;
    private bool inmunidad = false;
    public int defensiveAbilityHits = 0;

    private PlayerController playerController;

    void Start()
    {
        vidaActual = vidaMaxima;
        playerController = GetComponent<PlayerController>();

    }

    public void takeDamage(float damage)
    {
        if (inmunidad || playerController.isDashing) return;
        if (defensiveAbilityHits > 0)
        {
            defensiveAbilityHits--;
            if(defensiveAbilityHits == 0) playerController.ToggleShieldPrefab(false);
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
    if (Cronometro.instance != null)
    {
        Cronometro.instance.Detener();
    }

    StopAllCoroutines();

    // Mostrar cursor del sistema
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // Detener juego y mostrar menú derrota
    Time.timeScale = 0f;

    // Cargar escena de derrota de forma aditiva (para ver la mazmorra detrás)
    SceneManager.LoadScene("Derrota", LoadSceneMode.Additive);

    // Eliminar jugador
    Destroy(gameObject);
}

    IEnumerator ActivarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoInmunidad);
        inmunidad = false;
    }
}
