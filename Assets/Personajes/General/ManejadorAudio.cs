using UnityEngine;

public class ManejadorAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] sonidosDaño;
    [SerializeField] private AudioClip[] sonidosMuerte;
    [SerializeField] private AudioClip sonidoGolpe;
    [SerializeField] private AudioClip sonidoEquipar;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonidoDaño()
    {
        if (sonidosDaño.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, sonidosDaño.Length);
            audioSource.PlayOneShot(sonidosDaño[indiceAleatorio]);

        }
    }

    public void ReproducirSonidoMuerte()
    {
        if (sonidosMuerte.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, sonidosMuerte.Length);
            audioSource.PlayOneShot(sonidosMuerte[indiceAleatorio]);
        }
    }

    public void ReproducirSonidoEquipar()
    {
        if (sonidoEquipar)
        {
            audioSource.PlayOneShot(sonidoEquipar);
            
        }
    }

    public void ReproducirSonidoGolpe()
    {
        if (sonidoGolpe)
        {
            audioSource.PlayOneShot(sonidoGolpe);
        }
    }
}


