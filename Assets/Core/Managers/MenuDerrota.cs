using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{
    public void Reintentar()
    {
        Time.timeScale = 1f;

        GameManager.instance.playerSpawned = false; // Necesario para que se instancie
        GameManager.instance.isLevelLoaded = false;

        SceneManager.LoadScene("Mazmorra1"); // Esto hará que LevelManager vuelva a cargar
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