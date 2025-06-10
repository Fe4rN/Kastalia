using UnityEngine;
using UnityEngine.UI;

public class Menu_Victoria : MonoBehaviour
{
    [SerializeField] Button volverAlMenuButton;
    [SerializeField] Button jugarDeNuevoButton;
    [SerializeField] Button salirDelJuegoButton;

    void Start()
    {
        volverAlMenuButton.onClick.AddListener(VolverAlMenu);
        jugarDeNuevoButton.onClick.AddListener(JugarDeNuevo);
        salirDelJuegoButton.onClick.AddListener(SalirDelJuego);
    }


    public void VolverAlMenu()
    {
        var source = GameManager.instance.GetComponent<AudioSource>();
        if (source != null && source.isPlaying)
            source.Stop();
        GameManager.instance.StartMainGameLoop();
    }
    public void JugarDeNuevo()
    {
        var source = GameManager.instance.GetComponent<AudioSource>();
        if (source != null && source.isPlaying)
            source.Stop();
        GameManager.instance.StartMainGameLoop();
    }

    public void SalirDelJuego()
    {
        var source = GameManager.instance.GetComponent<AudioSource>();
        if (source != null && source.isPlaying)
            source.Stop();
        GameManager.instance.QuitGame();
    }
}