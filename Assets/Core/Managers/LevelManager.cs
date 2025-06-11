using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject drevenPrefab;
    [SerializeField] private GameObject lyxPrefab;

    public GameObject player;
    public Vector3 spawnPoint;
    public GameObject UI;

    private CinemachineCamera cinemachineCamera;

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
        }
    }

    void Start()
    {
        switch (GameManager.instance.personaje)
        {
            case Characters.Dreven:
                player = Instantiate(drevenPrefab, spawnPoint, Quaternion.identity);
                break;
            case Characters.Lyx:
                player = Instantiate(lyxPrefab, spawnPoint, Quaternion.identity);
                break;
            default:
                GameManager.instance.CargarMenuPrincipal();
                return;
        }

        CinemachineCamera camera = FindFirstObjectByType<CinemachineCamera>();
        camera.Follow = player.transform;
        Instantiate(UI, Vector3.zero, Quaternion.identity);
    }
}