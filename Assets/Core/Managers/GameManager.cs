using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public int characterIndex = -1;
    public bool playerSpawned = false;
    public string currentCharacter;
    // Personajes

    [SerializeField] private GameObject Lyx;
    [SerializeField] private GameObject Dreven;

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
        if (characterIndex != -1 && !playerSpawned)
        {
            switch (characterIndex)
            {
                case 1:
                    player = Instantiate(Lyx, Vector3.zero, Quaternion.identity);
                    currentCharacter = "Lyx";
                    playerSpawned = true;
                    SceneManager.UnloadSceneAsync("CharacterSelection");
                    break;
                case 2:
                    player = Instantiate(Dreven, Vector3.zero, Quaternion.identity);
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
}
