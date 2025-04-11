using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int enemyQuantity;
    [SerializeField] private GameObject enemyPrefab;
    public static LevelManager instance;

    public GameObject player;
    public Vector3 spawnPoint;

    private CinemachineCamera cinemachineCamera;
    private Vector3 EnemySpawnPoint = new Vector3(6.95f, 0, 23.78f);

    public bool isLevelLoaded = false;

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
        string escenaActual = SceneManager.GetActiveScene().name;

        if (escenaActual == "Derrota" || escenaActual == "Victoria")
        {
            Destroy(gameObject);
            return;
        }

        isLevelLoaded = false;
        GameManager.instance.playerSpawned = false;
    }

    void Update()
    {
        if (!SceneManager.GetSceneByName("Mazmorra1").isLoaded || isLevelLoaded)
            return;

        if (!isLevelLoaded && GameManager.instance.personajeSeleccionado != null && !GameManager.instance.playerSpawned)
        {
            LoadLevel();
            LoadPlayer(); // instanciamos siempre desde el prefab guardado
            GameManager.instance.playerSpawned = true;
            isLevelLoaded = true;
        }
    }

    public void LoadLevel()
    {
        LoadEnemies();
        GameObject spawnObj = GameObject.Find("SpawnPoint");
        spawnPoint = spawnObj ? spawnObj.transform.position : Vector3.zero;
    }

    public void LoadPlayer()
    {
        GameObject prefab = GameManager.instance.personajeSeleccionado;

        if (prefab == null)
        {
            Debug.LogWarning("El prefab del personaje no está asignado en GameManager.");
            return;
        }

        player = Instantiate(prefab, spawnPoint - new Vector3(0, 1, 0), Quaternion.identity);

        cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
        if (cinemachineCamera != null && player != null)
        {
            cinemachineCamera.Follow = player.transform;
            Debug.Log("Cámara siguiendo al jugador");
        }
    }

    private void LoadEnemies()
    {
        for (int i = 0; i < enemyQuantity; i++)
        {
            Instantiate(enemyPrefab, EnemySpawnPoint, Quaternion.identity);
        }
    }

    public void ResetLevelState(bool reiniciarJugador = false)
    {
        isLevelLoaded = false;

        if (reiniciarJugador)
            player = null;
    }

    public void ReloadMazmorraLevel()
    {
        ResetLevelState();
        SceneManager.LoadScene("Mazmorra1");
    }
}
