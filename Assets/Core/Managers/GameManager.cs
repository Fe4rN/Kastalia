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

    // 🆕 NUEVO: Referencias a los prefabs de armas
    [SerializeField] private GameObject prefabHojaAfilada;
    [SerializeField] private GameObject prefabArco;

    public GameObject UI;

    public GameObject personajeSeleccionado;


    [SerializeField] private float transitionDuration = 0.5f;

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
    }

    void Start()
    {
        StartMainMenu();
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
        isPaused = true;
        SceneManager.LoadSceneAsync("Menu_Victoria", LoadSceneMode.Additive);
    }

    private IEnumerator CargarMazmorraYSeleccion()
    {
        yield return SceneManager.LoadSceneAsync("Mazmorra1");
        yield return SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
    }

    public void PauseGame()
    {

        // Disable the main Audio Listener before loading pause menu
        AudioListener mainListener = FindObjectOfType<AudioListener>();
        if (mainListener != null)
        {
            mainListener.enabled = false;
        }

        StartCoroutine(LoadSceneWithTransition("PauseMenu", true));
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        StartCoroutine(UnloadSceneWithTransition("PauseMenu"));
        Time.timeScale = 1f;
        isPaused = false;

        // Re-enable the main Audio Listener after unloading pause menu
        AudioListener mainListener = FindObjectOfType<AudioListener>();
        if (mainListener != null)
        {
            mainListener.enabled = true;
        }
    }

    public void StartMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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

        SceneManager.LoadScene("MainMenu");
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


    // New transition methods
    private IEnumerator LoadSceneWithTransition(string sceneName, bool additive)
    {
        yield return SceneManager.LoadSceneAsync("Transition", additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        yield return new WaitForSecondsRealtime(transitionDuration);

        // Load the target scene
        if (additive)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }

        if (additive)
        {
            yield return SceneManager.UnloadSceneAsync("Transition");
        }
    }
    

    private IEnumerator UnloadSceneWithTransition(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync("Transition", LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(transitionDuration);
        yield return SceneManager.UnloadSceneAsync(sceneName);
        yield return SceneManager.UnloadSceneAsync("Transition");
    }
}
