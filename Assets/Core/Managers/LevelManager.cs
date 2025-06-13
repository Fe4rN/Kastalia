using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using Unity.Multiplayer.Center.Common;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject drevenPrefab;
    [SerializeField] private GameObject lyxPrefab;

    [SerializeField] private GameObject arcoPrefab;
    [SerializeField] private GameObject hojaAfiladaPrefab;
    private Vector3 puntoAparicionArma = new Vector3(-0.62f, 0.03f, -5.75f);

    public GameObject player;
    public Vector3 spawnPoint;
    public GameObject UIprefab;
    public GameObject UI;

    private CinemachineCamera cinemachineCamera;

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
        if(GameManager.instance.personaje == Characters.None) InitLevel();
    }

    public void InitLevel()
    {
        switch (GameManager.instance.personaje)
        {
            case Characters.Dreven:
                player = Instantiate(drevenPrefab, spawnPoint, Quaternion.identity);
                Instantiate(arcoPrefab, puntoAparicionArma, Quaternion.identity);
                break;
            case Characters.Lyx:
                player = Instantiate(lyxPrefab, spawnPoint, Quaternion.identity);
                Instantiate(hojaAfiladaPrefab, puntoAparicionArma, Quaternion.identity);
                break;
            default:
                GameManager.instance.CargarMenuPrincipal();
                return;
        }
        CinemachineCamera camera = FindFirstObjectByType<CinemachineCamera>();
        camera.Follow = player.transform;
        UI = Instantiate(UIprefab, Vector3.zero, Quaternion.identity);
    }
}