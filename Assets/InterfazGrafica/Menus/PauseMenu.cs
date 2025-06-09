using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resume;
    [SerializeField] Button quit;
    [SerializeField] Button backToMenu;
   

    void Start()
    {

        resume.onClick.AddListener(() => {
            StopPauseMusic();
            GameManager.instance.ResumeGame();
        });
        quit.onClick.AddListener(() => CloseGame());
        backToMenu.onClick.AddListener(() => {
            StopPauseMusic();
            BackToMenu();
        });
    }
    private void StopPauseMusic()
    {
        var src = GameManager.instance.GetComponent<AudioSource>();
        if (src != null && src.isPlaying)
            src.Stop();
    }

    private void BackToMenu()
    {


        Time.timeScale = 1f;
        GameManager.instance.StartMainGameLoop();
    }

    private void CloseGame(){
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}

