using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public int enemyQuantity;
    public bool isPlayerLoadedIn = false;
    public bool isLevelLoaded = false;
    private CinemachineCamera cinemachineCamera;
    private Vector3 EnemySpawnPoint = new Vector3(6.95f, 0, 23.78f);
    private bool canEnemySpawn = false;
    [SerializeField] private GameObject enemyPrefab;
    public static LevelManager instance;
    public GameObject player;
    public Vector3 spawnPoint;
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
        canEnemySpawn = true;
    }

    void Update()
    {
        if(isLevelLoaded) return;
        if(GameManager.instance.playerSpawned){
            LoadLevel();
            isLevelLoaded = true;
        }
    }

    public void LoadLevel()
    {
        LoadEnemies();
        spawnPoint = GameObject.Find("SpawnPoint").transform.position;
        LoadPlayer();
    }

    public void LoadPlayer()
    {
        if(GameManager.instance.currentCharacter == "Lyx") player = Instantiate(GameManager.instance.Lyx, spawnPoint - new Vector3(0,1,0), Quaternion.identity);
        if(GameManager.instance.currentCharacter == "Dreven") player = Instantiate(GameManager.instance.Dreven, spawnPoint - new Vector3(0,1,0), Quaternion.identity);
        cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
        cinemachineCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;

        
    }
    private void LoadEnemies()
    {
        if(canEnemySpawn){
            StartCoroutine(enemySpawnCooldown());
            for (int i = 0; i < enemyQuantity; i++)
            {
                Instantiate(enemyPrefab, EnemySpawnPoint, Quaternion.identity);
            }
        }
    }


    IEnumerator enemySpawnCooldown(){
        canEnemySpawn = false;
        yield return new WaitForSeconds(5);
        canEnemySpawn = true;
    }
    
}
