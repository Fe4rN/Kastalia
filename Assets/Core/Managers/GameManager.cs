using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public GameState gameState;
    public Characters personaje;
    public bool isPaused = false;

    public MainMenu menuPrincipal;
    public CharacterSelection menuSeleccionPersonaje;
    public GameObject menuOpciones;
    public MenuDerrota menuDerrota;
    public Menu_Victoria menuVictoria;
    public PauseMenu menuPausa;

    private GameObject menuPausaActivo;

    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    //Referencias a los prefabs de armas
    [SerializeField] private GameObject prefabHojaAfilada;
    [SerializeField] private GameObject prefabArco;

    //Audio
    [SerializeField] private AudioClip musicaVictoria;
    public AudioClip musicaDerrota;
    [SerializeField] private AudioClip pauseMenuClip;
    [SerializeField] private AudioClip musicaCombate;
    [SerializeField] private AudioClip musicaMenuPrincipal;

    [SerializeField] private AudioMixer audioMixer;

    public AudioSource audioSource;

    // Referencia a la UI del juego
    public GameObject UI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else DestroyImmediate(gameObject);

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) Debug.LogWarning("AudioSource no detectado");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Playing || gameState == GameState.Paused)
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
            else return;
        }
    }

    public void CargarMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
        gameState = GameState.MainMenu;
        audioSource.clip = musicaMenuPrincipal;
        audioSource.Play();
    }

    public void CargarMenuSeleccionPersonaje()
    {
        Time.timeScale = 1f;
        isPaused = false;
        personaje = Characters.None;
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            SceneLoader.instance.CargarEscena("MainMenu", () =>
            {
                audioSource.clip = musicaMenuPrincipal;
                audioSource.Play();
                menuPrincipal.gameObject.SetActive(false);
                menuOpciones.SetActive(false);
                menuSeleccionPersonaje.gameObject.SetActive(true);
                gameState = GameState.CharacterSelection;
            });
        }
        else
        {
            menuPrincipal.gameObject.SetActive(false);
            menuOpciones.SetActive(false);
            menuSeleccionPersonaje.gameObject.SetActive(true);
            gameState = GameState.CharacterSelection;
        }
    }

    public void IniciarPrimerNivel()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Mazmorra1");
        gameState = GameState.Playing;
        audioSource.clip = musicaCombate;
        audioSource.Play();

    }

    public void CargarMenuOpciones()
    {
        menuPrincipal.gameObject.SetActive(false);
        menuSeleccionPersonaje.gameObject.SetActive(false);
        menuOpciones.SetActive(true);
    }

    public void WinGame()
    {
        gameState = GameState.Victory;

        audioSource.clip = musicaVictoria;
        audioSource.Play();

        // Detener el cronómetro
        isPaused = true;
        Time.timeScale = 0f;// Pausar el tiempo para evitar problemas de sincronización

        LevelManager.instance.UI.SetActive(false);
        GameObject.FindAnyObjectByType<PlayerController>().animator.SetFloat("InputMagnitude", 0f);
        Instantiate(menuVictoria.gameObject, Vector3.zero, Quaternion.identity);
        gameState = GameState.Victory;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        gameState = GameState.Paused;
        audioMixer.SetFloat("MusicFocus", 500f);
        LevelManager.instance.UI.SetActive(false);

        menuPausaActivo = Instantiate(menuPausa).gameObject;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        gameState = GameState.Playing;
        audioMixer.SetFloat("MusicFocus", 22000f);
        LevelManager.instance.UI.SetActive(true);


        Destroy(menuPausaActivo);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    //Ya sé que esto mismo se encuentra en SceneLoader, pero es que estoy muy cansado (noche antes del Sprint final)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Mazmorra1")
        {
            LevelManager.instance.InitLevel();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}



public enum GameState
{
    MainMenu,
    CharacterSelection,
    Playing,
    Paused,
    Victory,
    Defeat
}