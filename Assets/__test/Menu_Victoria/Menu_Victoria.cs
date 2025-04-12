using UnityEngine;
using UnityEngine.UI;

public class Menu_Victoria : MonoBehaviour
{
    [SerializeField] Button volverAlMenuButton;
    [SerializeField] Button jugarDeNuevoButton;
    [SerializeField] Button salirDelJuegoButton;

    void Start()
    {
        volverAlMenuButton.onClick.AddListener(VolverAlMenu);
        jugarDeNuevoButton.onClick.AddListener(JugarDeNuevo);
        salirDelJuegoButton.onClick.AddListener(SalirDelJuego);
    }

    public void VolverAlMenu()
    {
        GameManager.instance.StartMainMenu();
    }
    public void JugarDeNuevo()
    {
        GameManager.instance.RestartGame();
    }

    public void SalirDelJuego() {
        GameManager.instance.SalirDelJuego();
    }
}