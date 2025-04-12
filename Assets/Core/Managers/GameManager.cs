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

        // 🔥 Eliminamos reinicio del cronómetro aquí para hacerlo tras selección real
        // if (IsSceneLoaded("Derrota") || IsSceneLoaded("MainMenu"))
        // {
        // }

        StartCoroutine(CargarMazmorraYSeleccion());
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
        playerSpawned = false;
        characterIndex = -1;
        LevelManager.instance.isLevelLoaded = false;
        StartCoroutine(UnloadAllAndLoadMainMenu());
    }

    public void WinGame()
    {
        isPaused = true;
        SceneManager.LoadScene("Menu_Victoria", LoadSceneMode.Additive);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        EditorApplication.isPlaying = false;

        Application.Quit();
    }

    private IEnumerator UnloadAllAndLoadMainMenu()
    {
        // First load MainMenu additively to avoid black screen
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("MainMenu");
        yield return loadOp;

        // Once loaded, unload all other scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "MainMenu")
            {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(scene);
                yield return unloadOp;
            }
        }

        // Finally, set MainMenu as active
        Scene mainMenuScene = SceneManager.GetSceneByName("MainMenu");
        if (mainMenuScene.IsValid())
        {
            SceneManager.SetActiveScene(mainMenuScene);
        }
    }
}
