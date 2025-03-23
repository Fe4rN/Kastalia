using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void EscenaJuego()
    {
        SceneManager.LoadScene("Mazmorra1");
    }

    public void CargarNivel(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }
    public void Salir()
    {
        Application.Quit();
    }
}
