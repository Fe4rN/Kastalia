using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int characterIndex = -1;
    public int lastCharacterIndex = -1;
    public bool playerSpawned = false;
    public string currentCharacter;

    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    public GameObject personajeSeleccionado;

    public bool isPaused = false;
    public bool isLevelLoaded = false;

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

    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync("Mazmorra1");
        playerSpawned = false;
        StartMainGameLoop();
    }

    public void StartMainGameLoop()
    {
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

    private void StartMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;

        characterIndex = lastCharacterIndex;
        playerSpawned = false;
        isLevelLoaded = false;

        SceneManager.LoadScene("Mazmorra1"); // el prefab sigue guardado en personajeSeleccionado
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

        SceneManager.LoadScene("MainMenu");
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
                return true;
        }
        return false;
    }
}
