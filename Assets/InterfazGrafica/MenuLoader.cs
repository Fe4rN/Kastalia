using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] CharacterSelection characterSelection;

    void Start()
    {
        GameManager.instance.menuPrincipal = Instantiate(mainMenu);
        GameManager.instance.menuSeleccionPersonaje = Instantiate(characterSelection);
    }
}
