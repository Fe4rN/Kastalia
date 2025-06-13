using UnityEngine;
using UnityEngine.UI;

public class MenuDerrota : MonoBehaviour
{
    [SerializeField] private Button reintentarButton;
    [SerializeField] private Button menuPrincipalButton;
    [SerializeField] private Button salirButton;

    private void activarBotones() // Llamado desde la animacion de Mensaje
    {
        reintentarButton.gameObject.SetActive(true);
        menuPrincipalButton.gameObject.SetActive(true);
        salirButton.gameObject.SetActive(true);
    }

    public void Reintentar()
    {
        GameManager.instance.CargarMenuSeleccionPersonaje();
    }

    public void MenuPrincipal()
    {
        GameManager.instance.CargarMenuPrincipal();
    }

    public void Salir()
    {
        GameManager.instance.QuitGame();
    }
}
