using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu_Pausa : MonoBehaviour
{
    public bool pause;
    public GameObject menuPausa;
    public static bool JuegoPausado = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            menuPausa.SetActive(pause);
        }
    }

    
    public void Pause()
    {
        pause = !pause;
        menuPausa.SetActive(pause);
        JuegoPausado = pause;
        Time.timeScale = pause ? 0 : 1;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

}
