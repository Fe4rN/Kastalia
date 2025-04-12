using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
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
            Debug.Log("Selected character: " + selectedCharacter);
            GameManager.instance.characterIndex = selectedCharacter;
            GameManager.instance.isPaused = false;
        } else {
            Debug.Log("No character selected");
        }
    }
}
