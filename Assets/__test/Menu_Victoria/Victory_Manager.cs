using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Victory_Manager : MonoBehaviour
{

    public void VolverAlMenu()
    {
        LevelManager.instance.isPlayerLoadedIn = false;
        GameManager.instance.StartMainMenu();
    }

    public void JugarDeNuevo()
    {
        SceneManager.UnloadSceneAsync("Menu_Victoria");
        GameManager.instance.RestartGame();
        GameManager.instance.isPaused = false;
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        EditorApplication.isPlaying = false;

        Application.Quit();

    }
}
