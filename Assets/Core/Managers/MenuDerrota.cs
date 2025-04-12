using UnityEngine;

public class MenuDerrota : MonoBehaviour
{
    public void Reintentar()
    {
        Time.timeScale = 1f;
        GameManager.instance.StartMainGameLoop();
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        GameManager.instance.VolverAlMenuPrincipal();
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}