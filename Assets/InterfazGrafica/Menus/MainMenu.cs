using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button jugar_boton;
    [SerializeField] private Button opciones_boton;
    [SerializeField] private Button salir_boton;

    public void boton_jugar_presionado()
    {
        GameManager.instance.CargarMenuSeleccionPersonaje();
    }

    public void boton_opciones_presionado()
    {
        GameManager.instance.CargarMenuOpciones();
    }
    
    public void boton_salir_presionado()
    {
        GameManager.instance.QuitGame();
    }
    
}