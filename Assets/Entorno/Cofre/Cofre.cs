using UnityEngine;

public class Cofre : MonoBehaviour
{
    [SerializeField] private GameObject[] Loot;
    [SerializeField] private Transform puntoDeAparicion;
    [SerializeField] private Animator animator;

    private bool jugadorCerca = false;
    private bool estaAbierto = false;

    private void Start()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (jugadorCerca && !estaAbierto && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        estaAbierto = true;
        animator.SetTrigger("abrir");
        MainInterface hud = FindFirstObjectByType<MainInterface>();
        if (hud) hud.cambiarBotonInteraccion(false);

        escogerLootdePool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(estaAbierto) return;
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            MainInterface hud = FindFirstObjectByType<MainInterface>();
            if (hud) hud.cambiarBotonInteraccion(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(estaAbierto) return;
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            MainInterface hud = FindFirstObjectByType<MainInterface>();
            if (hud) hud.cambiarBotonInteraccion(false);
        }
    }

    private void escogerLootdePool()
    {
        int indiceAleatorio = Random.Range(0, Loot.Length);
        GameObject objetoAleatorio = Loot[indiceAleatorio];
        if (objetoAleatorio && puntoDeAparicion)
        {
            Instantiate(objetoAleatorio, puntoDeAparicion.position, Quaternion.identity);
            MainInterface hud = FindFirstObjectByType<MainInterface>();
            if (hud)
            {
                hud.DispararNotificacion(objetoAleatorio.name);
            }
        }
    }
}
