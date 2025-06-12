using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button reanudarBoton;
    [SerializeField] Button menuBoton;
    [SerializeField] Button salirBoton;

    public void Reanudar()
    {
        GameManager.instance.ResumeGame();
    }

    public void Menu()
    {
        GameManager.instance.CargarMenuPrincipal();
    }

    public void Salir()
    {
        GameManager.instance.QuitGame();
    }
}