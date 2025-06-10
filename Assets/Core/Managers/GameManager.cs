using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int characterIndex = -1;
    public bool playerSpawned = false;
    public bool isPaused = false;
    public bool isLevelLoaded = false;


    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    //NUEVO: Referencias a los prefabs de armas
    [SerializeField] private GameObject prefabHojaAfilada;
    [SerializeField] private GameObject prefabArco;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    //NUevo: Audio
    [SerializeField] private AudioClip victoriaClip;
    [SerializeField] private AudioClip derrotaMenuClip;
    [SerializeField] private AudioClip pauseMenuClip;
    [SerializeField] private AudioClip backgroundMusicClip;

    private AudioSource audioSource;

    // Referencia a la UI del juego
    public GameObject UI;

    public GameObject personajeSeleccionado;

    [SerializeField] private float fadeDuration = 0.5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        StartCoroutine(DescargarTodasLasEscenas());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void StartMainGameLoop()
    {

        if (backgroundMusicClip != null && audioSource != null)
        {
            audioSource.clip = backgroundMusicClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        characterIndex = -1;
        personajeSeleccionado = null;
        playerSpawned = false;
        isLevelLoaded = false;

        if (LevelManager.instance != null)
            LevelManager.instance.ResetLevelState(true);
        if (LevelManager.instance != null)
            LevelManager.instance.ResetLevelState(true);

        // Limpiar cofres anteriores
        ItemDropTracker.Reiniciar();
        StartCoroutine(CargarMazmorraYSeleccion());
    }

    public void WinGame()
    {
        //Reproducir el clip de victoria
        if (victoriaClip != null && audioSource != null)
            audioSource.PlayOneShot(victoriaClip);

        // Detener el cronómetro
        isPaused = true;
        Time.timeScale = 0f;// Pausar el tiempo para evitar problemas de sincronización

        StartCoroutine(LoadSceneWithTransition("Menu_Victoria", true));
    }
    public void LoseGame()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        if (derrotaMenuClip != null && audioSource != null)
            audioSource.PlayOneShot(derrotaMenuClip);

        isPaused = true;
        Time.timeScale = 0f;

        StartCoroutine(LoadSceneWithTransition("Derrota", true));
    }



    private IEnumerator CargarMazmorraYSeleccion()
    {

        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        AsyncOperation loadMazmorra = SceneManager.LoadSceneAsync("Mazmorra1");
        yield return loadMazmorra;

        fadeObject = CreateFadeOverlay();
        fadeGroup = fadeObject.GetComponent<CanvasGroup>();
        fadeGroup.alpha = 1f; // Start from black
        AsyncOperation loadSelection = SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
        yield return loadSelection;
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        Destroy(fadeObject);
    }

    public void PauseGame()
    {

        StartCoroutine(LoadSceneWithTransition("PauseMenu", true));
        Time.timeScale = 0f;
        isPaused = true;

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Pause();


        // Reproducir el clip de pausa:
        if (pauseMenuClip != null && audioSource != null)
            audioSource.PlayOneShot(pauseMenuClip);
    }

    public void ResumeGame()
    {
        StartCoroutine(UnloadSceneWithTransition("PauseMenu"));
        Time.timeScale = 1f;

        if (audioSource != null && !audioSource.isPlaying)
            audioSource.UnPause();

        //Detener música de pausa
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        isPaused = false;
    }

    private IEnumerator DescargarTodasLasEscenas()
    {
        int cuentaEscenas = SceneManager.sceneCount;
        Scene escenaActiva = SceneManager.GetActiveScene();

        for (int numero_escena = 0; numero_escena < cuentaEscenas; numero_escena++)
        {
            Scene escena_actual = SceneManager.GetSceneAt(numero_escena);

            if (escena_actual != escenaActiva)
            {
                AsyncOperation descargaAsincrona = SceneManager.UnloadSceneAsync(escena_actual);

                while (!descargaAsincrona.isDone)
                {
                    yield return null;
                }
            }
        }
        SceneManager.UnloadSceneAsync(escenaActiva);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void VolverAlMenuPrincipal()
    {
        characterIndex = -1;
        personajeSeleccionado = null;
        playerSpawned = false;
        isPaused = false;
        isLevelLoaded = false;

        if (LevelManager.instance != null)
        {
            LevelManager.instance.ResetLevelState(true);
        }

        if (Cronometro.instance != null)
        {
            Cronometro.instance.ReiniciarCronometro();
        }

        // 🔁 Limpiar ítems de cofres al volver al menú principal
        ItemDropTracker.Reiniciar();

        StartCoroutine(LoadSceneWithTransition("MainMenu", false));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // 🆕 NUEVO: Método para instanciar el arma cerca del personaje seleccionado
    public void InstanciarArmaParaPersonaje()
    {
        if (personajeSeleccionado == null) return;

        Vector3 posicionFrente = personajeSeleccionado.transform.position + personajeSeleccionado.transform.forward * 1f;

        if (personajeSeleccionado == Lyx && prefabHojaAfilada != null)
        {
            Instantiate(prefabHojaAfilada, posicionFrente, Quaternion.identity);
        }
        else if (personajeSeleccionado == Dreven && prefabArco != null)
        {
            Instantiate(prefabArco, posicionFrente, Quaternion.identity);
        }
    }

    // 🆕 NUEVO: Método sugerido para ser llamado después de seleccionar personaje
    public void SeleccionarPersonaje(GameObject personaje)
    {
        personajeSeleccionado = personaje;
        playerSpawned = true;

        InstanciarArmaParaPersonaje(); // 🆕 Instancia el arma correspondiente
    }


    // -----------------------------------------------------------------------
    // -------------------Metodos Para Transiciones entre menus ----------------
    // -----------------------------------------------------------------------

    private IEnumerator LoadSceneWithTransition(string sceneName, bool additive)
    {
        // Create a simple fade overlay
        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();

        // Fade in (to black)
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        // Store reference before potential destruction
        var fadeObjectToDestroy = fadeObject;

        // Load the target scene
        AsyncOperation asyncLoad;
        if (additive)
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            // When loading a new scene non-additively, our fade object will be destroyed
            // so we need to recreate it after scene load
            asyncLoad.completed += (op) =>
            {
                if (fadeObjectToDestroy != null)
                    Destroy(fadeObjectToDestroy);
            };
        }

        yield return asyncLoad;

        // For non-additive loads, recreate the fade object
        if (!additive)
        {
            fadeObject = CreateFadeOverlay();
            fadeGroup = fadeObject.GetComponent<CanvasGroup>();
            fadeGroup.alpha = 1f; // Start from black
        }

        // Fade out (to clear)
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        // Clean up
        if (fadeObject != null)
            Destroy(fadeObject);
    }

    private IEnumerator UnloadSceneWithTransition(string sceneName)
    {
        // Create a simple fade overlay
        GameObject fadeObject = CreateFadeOverlay();
        CanvasGroup fadeGroup = fadeObject.GetComponent<CanvasGroup>();

        // Fade in (to black)
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        // Unload the scene
        yield return SceneManager.UnloadSceneAsync(sceneName);

        // Fade out (to clear)
        yield return Fade(fadeGroup, 1f, 0f, fadeDuration);

        // Clean up
        Destroy(fadeObject);
    }

    // Helper method to create a simple fade overlay
    private GameObject CreateFadeOverlay()
    {
        GameObject fadeObject = new GameObject("FadeOverlay");

        // Setup Canvas
        Canvas canvas = fadeObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Make sure it's on top

        // Setup CanvasGroup for fading
        CanvasGroup group = fadeObject.AddComponent<CanvasGroup>();

        // Create full-screen image
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeObject.transform);
        UnityEngine.UI.Image image = imageObject.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;

        // Stretch to full screen
        RectTransform rect = imageObject.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return fadeObject;
    }

    // Helper method to handle fading
    private IEnumerator Fade(CanvasGroup group, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        group.alpha = endAlpha;
    }
}