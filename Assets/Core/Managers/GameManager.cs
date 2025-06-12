using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public Characters personaje;
    public bool isPaused = false;

    public MainMenu menuPrincipal;
    public CharacterSelection menuSeleccionPersonaje;
    public GameObject menuOpciones;



    [SerializeField] public GameObject Lyx;
    [SerializeField] public GameObject Dreven;

    //Referencias a los prefabs de armas
    [SerializeField] private GameObject prefabHojaAfilada;
    [SerializeField] private GameObject prefabArco;

    //Audio
    [SerializeField] private AudioClip victoriaClip;
    [SerializeField] private AudioClip derrotaMenuClip;
    [SerializeField] private AudioClip pauseMenuClip;
    [SerializeField] private AudioClip backgroundMusicClip;

    private AudioSource audioSource;

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

        // audioSource = GetComponent<AudioSource>();
        // audioSource.playOnAwake = false;
        // audioSource.loop = false;
    }

    // void Start()
    // {
    //     menuPrincipal = FindFirstObjectByType<MainMenu>(FindObjectsInactive.Include).gameObject;
    //     menuSeleccionPersonaje = FindFirstObjectByType<CharacterSelection>(FindObjectsInactive.Include).gameObject;
    //     CargarMenuPrincipal();
    // }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void CargarMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CargarMenuSeleccionPersonaje()
    {
        menuPrincipal.gameObject.SetActive(false);
        menuSeleccionPersonaje.gameObject.SetActive(true);
    }

    public void IniciarPrimerNivel()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Mazmorra1");

    }

    public void CargarMenuOpciones()
    {

    }

    public void WinGame()
    {
        //Reproducir el clip de victoria
        if (victoriaClip != null && audioSource != null)
            audioSource.PlayOneShot(victoriaClip);

        // Detener el cronómetro
        isPaused = true;
        Time.timeScale = 0f;// Pausar el tiempo para evitar problemas de sincronización
    }
    public void LoseGame()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        if (derrotaMenuClip != null && audioSource != null)
            audioSource.PlayOneShot(derrotaMenuClip);

        isPaused = true;
        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
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
        Time.timeScale = 1f;

        if (audioSource != null && !audioSource.isPlaying)
            audioSource.UnPause();

        //Detener música de pausa
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        isPaused = false;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Mazmorra1")
        {
            LevelManager.instance.InitLevel();
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}