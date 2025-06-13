using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private Button Dreven_boton;
    [SerializeField] private Button Lyx_boton;
    [SerializeField] private Button Confirmar_boton;

    private Characters personajeSeleccionado;

    void OnEnable()
    {
        Dreven_boton.onClick.AddListener(() => SeleccionarPersonaje(Characters.Dreven));
        Lyx_boton.onClick.AddListener(() => SeleccionarPersonaje(Characters.Lyx));
        personajeSeleccionado = Characters.None;
        GameManager.instance.personaje = Characters.None;
        if (Confirmar_boton.interactable) Confirmar_boton.interactable = false;
    }

    private void SeleccionarPersonaje(Characters personaje)
    {
        personajeSeleccionado = personaje;
        ActivarBotonConfirmar();
    }

    private void ActivarBotonConfirmar()
    {
        Confirmar_boton.interactable = true;
        Confirmar_boton.onClick.AddListener(() => ConfirmarSeleccion());
    }
    
    private void ConfirmarSeleccion()
    {
        if (personajeSeleccionado == Characters.Dreven || personajeSeleccionado == Characters.Lyx)
        {
            GameManager.instance.personaje = personajeSeleccionado;
            GameManager.instance.IniciarPrimerNivel();
        }
    }
}