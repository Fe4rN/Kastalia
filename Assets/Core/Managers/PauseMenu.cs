using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Button resume;
    Button quit;

    void Start()
    {
        resume = GameObject.Find("Play").GetComponent<Button>();
        quit = GameObject.Find("Exit").GetComponent<Button>();
        resume.onClick.AddListener(() => GameManager.instance.ResumeGame());
        quit.onClick.AddListener(() => CloseGame());
    }

    private void CloseGame(){
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

