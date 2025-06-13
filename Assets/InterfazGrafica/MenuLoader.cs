using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] CharacterSelection characterSelection;
    [SerializeField] GameObject opcionesMenu;

    void Start()
    {
        GameManager.instance.menuPrincipal = Instantiate(mainMenu);
        GameManager.instance.menuSeleccionPersonaje = Instantiate(characterSelection);
        GameManager.instance.menuOpciones = Instantiate(opcionesMenu);
    }
}
