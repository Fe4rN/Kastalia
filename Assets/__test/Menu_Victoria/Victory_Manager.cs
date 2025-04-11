using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Victory_Manager : MonoBehaviour
{
    public GameObject winMenu;

    private void Start()
    {
        if (winMenu != null)
            winMenu.SetActive(false);
    }

    public void WinGame()
    {
        Debug.Log("1");
        Time.timeScale = 0;

        Debug.Log("2");
        if (winMenu != null) { winMenu.SetActive(true); }


        Debug.Log("3");
        SceneManager.LoadScene("Menu_Victoria"); 
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void JugarDeNuevo()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        EditorApplication.isPlaying = false;

        Application.Quit();

    }
}
