using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public Button Lyx;
    public Button Dreven;
    public Button Confirm;

    private Button selectedButton;
    private int selectedCharacter = -1;

    void Start()
    {
        Lyx.onClick.AddListener(() => selectCharacter(1));
        Dreven.onClick.AddListener(() => selectCharacter(2));
        Confirm.onClick.AddListener(confirmSelection);
    }

    private void selectCharacter(int value)
    {
        selectedCharacter = value;
        selectedButton = (value == 1) ? Lyx : Dreven;
    }

    void confirmSelection()
    {
        if (selectedCharacter != -1)
        {
            GameManager.instance.characterIndex = selectedCharacter;
            GameManager.instance.lastCharacterIndex = selectedCharacter;

            if (selectedCharacter == 1)
                GameManager.instance.personajeSeleccionado = GameManager.instance.Lyx;
            else if (selectedCharacter == 2)
                GameManager.instance.personajeSeleccionado = GameManager.instance.Dreven;

            if (SceneManager.GetSceneByName("CharacterSelection").isLoaded)
            {
                SceneManager.UnloadSceneAsync("CharacterSelection");
            }
        }
        else
        {
            Debug.Log("No character selected");
        }
    }
}
