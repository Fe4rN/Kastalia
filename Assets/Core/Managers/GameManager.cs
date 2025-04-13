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

    public GameObject personajeSeleccionado;

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

    public void WinGame(){
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
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
        isPaused = false;
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
}
