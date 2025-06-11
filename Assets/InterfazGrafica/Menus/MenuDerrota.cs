using UnityEngine;

public class MenuDerrota : MonoBehaviour
{
    public void Reintentar()
    {
        Time.timeScale = 1f;
        var gmSource = GameManager.instance.GetComponent<AudioSource>();
        if (gmSource != null && gmSource.isPlaying)
            gmSource.Stop();
    }

    public void IrAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        var gmSource = GameManager.instance.GetComponent<AudioSource>();
        if (gmSource != null && gmSource.isPlaying)
            gmSource.Stop();
    }

    public void SalirDelJuego()
    {
        Application.Quit();
    }
}