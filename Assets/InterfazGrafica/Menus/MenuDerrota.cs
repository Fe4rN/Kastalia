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
        GameManager.instance.StartMainGameLoop();
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}