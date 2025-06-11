using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Victory_Manager : MonoBehaviour
{

    public void VolverAlMenu()
    {
        GameManager.instance.CargarMenuPrincipal();
    }

    public void JugarDeNuevo()
    {
        SceneManager.UnloadSceneAsync("Menu_Victoria");
        GameManager.instance.isPaused = false;
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();

    }
}
