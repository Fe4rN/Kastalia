using UnityEngine;
using UnityEngine.UI;

public class Menu_Victoria : MonoBehaviour
{
    [SerializeField] private Button menuBoton;
    [SerializeField] private Button salirBoton;

    public void habilitarBotonMenu()
    {
        menuBoton.gameObject.SetActive(true);
    }

    public void habilitarBotonSalir()
    {
        salirBoton.gameObject.SetActive(true);
    }

    public void VolverAlMenu()
    {
        GameManager.instance.CargarMenuPrincipal();
    }

    public void salirDelJuego()
    {
        GameManager.instance.QuitGame();
    }
}
