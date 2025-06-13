using TMPro;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private const string WINDOW_MODE_KEY = "WindowMode";

    void Start()
    {
        int savedMode = PlayerPrefs.GetInt(WINDOW_MODE_KEY, (int)Screen.fullScreenMode);
        dropdown.value = savedMode;
        dropdown.RefreshShownValue();

        ApplyWindowMode(savedMode);
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        ApplyWindowMode(index);
        PlayerPrefs.SetInt(WINDOW_MODE_KEY, index);
        PlayerPrefs.Save();
    }

    private void ApplyWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
    }
}
