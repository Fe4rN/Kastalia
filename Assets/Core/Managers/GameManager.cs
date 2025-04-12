using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public int characterIndex = -1;
    public bool playerSpawned = false;
    public string currentCharacter;
    // Personajes

    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    public bool isPaused = false;
    private bool isFromMainMenu = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
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
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        if (characterIndex != -1 && !playerSpawned)
        {
            switch (characterIndex)
            {
                case 1:
                    currentCharacter = "Lyx";
                    playerSpawned = true;
                    SceneManager.UnloadSceneAsync("CharacterSelection");
                    break;
                case 2:
                    currentCharacter = "Dreven";
                    playerSpawned = true;
                    SceneManager.UnloadSceneAsync("CharacterSelection");
                    break;
                default:
                    playerSpawned = false;
                    break;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync("Mazmorra1");
        characterIndex = -1;
        playerSpawned = false;
        LevelManager.instance.isLevelLoaded = false;
        StartMainGameLoop();
    }

    public void StartMainGameLoop()
    {
        if (isFromMainMenu)
        {
            SceneManager.UnloadSceneAsync("MainMenu");
            isFromMainMenu = false;
        }
        SceneManager.LoadSceneAsync("Mazmorra1");
        SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
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
