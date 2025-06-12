using System.Collections;
using Unity.Mathematics;
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
    private MainInterface mainInterface;
    private Animator animator;
    private Healthbar healthbar;
    private ManejadorAudio manejadorAudio;

    [SerializeField] private GameObject deathCameraPrefab;

    void Start()
    {
        vidaActual = vidaMaxima;
        playerController = GetComponent<PlayerController>();
        mainInterface = FindFirstObjectByType<MainInterface>();
        healthbar = FindFirstObjectByType<Healthbar>();
        animator = GameObject.Find("Main Camera").GetComponent<Animator>();
        manejadorAudio = GetComponent<ManejadorAudio>();
    }

    public void takeDamage(float damage)
    {
        if (inmunidad || playerController.isDashing) return;
        if (defensiveAbilityHits > 0)
        {
            defensiveAbilityHits--;
            if (defensiveAbilityHits == 0) playerController.ToggleShieldPrefab(false);
            StartCoroutine(ActivarInmunidad());
            return;
        }
        if (vidaActual - damage > 0)
        {
            vidaActual -= damage;
            animator.SetTrigger("Hurt");
            manejadorAudio.ReproducirSonidoDaño();
            healthbar.UpdateHealthbar(vidaMaxima, vidaActual, false);
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

        healthbar.UpdateHealthbar(vidaMaxima, vidaActual, true);
    }

    public void Die()
    {

        if (Cronometro.instance != null)
        {
            Cronometro.instance.Detener();
        }

        StopAllCoroutines();
        GameManager.instance.audioSource.clip = GameManager.instance.musicaDerrota;
        GameManager.instance.audioSource.Play();
        LevelManager.instance.UI.SetActive(false);
        manejadorAudio.ReproducirSonidoMuerte();

        // Mostrar cursor del sistema
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Freeze game time
        GameManager.instance.isPaused = true;
        Time.timeScale = 0f;


        // Set animator to unscaled time
        playerController.animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        GameObject activeCamera = GameObject.Find("CinemachineCamera");
        Camera.main.gameObject.SetActive(false);
        GameObject deathCamera = Instantiate(deathCameraPrefab, activeCamera.transform.position, quaternion.identity);
        deathCamera.transform.LookAt(playerController.transform.position);

        // Trigger death animation
        playerController.animator.SetTrigger("Death");

        // Start coroutine to wait for animation and load scene
        StartCoroutine(WaitForDeathAnimation());
    }

    IEnumerator ActivarInmunidad()
    {
        inmunidad = true;
        yield return new WaitForSeconds(tiempoInmunidad);
        inmunidad = false;
    }

    private IEnumerator WaitForDeathAnimation()
    {
        Animator animator = playerController.animator;

        // Wait until the animator is in the Death state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            yield return null;

        // Wait until animation finishes (using unscaled time)
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0f;

        while (timer < animationLength)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Instantiate(GameManager.instance.menuDerrota, Vector3.zero, Quaternion.identity);
        GameManager.instance.gameState = GameState.Defeat;
    }
}
