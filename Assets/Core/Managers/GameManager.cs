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
    public string LoadMode;

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
        LoadScenes();
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
        LoadScenes();
    }

    public void LoadScenes(){
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
}
