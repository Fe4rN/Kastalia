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
        Debug.Log("Personaje seleccionado: " + GameManager.instance.personaje);
        switch (GameManager.instance.personaje)
        {
            case Characters.Dreven:
                player = Instantiate(drevenPrefab, spawnPoint, Quaternion.identity);
                Instantiate(arcoPrefab, puntoAparicionArma, Quaternion.identity);
                Debug.Log("Dreven seleccionado");
                break;
            case Characters.Lyx:
                player = Instantiate(lyxPrefab, spawnPoint, Quaternion.identity);
                Instantiate(hojaAfiladaPrefab, puntoAparicionArma, Quaternion.identity);
                Debug.Log("Lyx seleccionado");
                break;
            default:
                GameManager.instance.CargarMenuPrincipal();
                return;
        }

        Debug.Log("Player instantiated: " + player.name);
        CinemachineCamera camera = FindFirstObjectByType<CinemachineCamera>();
        camera.Follow = player.transform;
        Instantiate(UI, Vector3.zero, Quaternion.identity);
    }
}