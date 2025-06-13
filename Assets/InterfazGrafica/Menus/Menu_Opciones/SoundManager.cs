using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderEffects;

    [SerializeField] private TMP_Text MasterVolume;
    [SerializeField] private TMP_Text MusicVolume;
    [SerializeField] private TMP_Text EffectsVolume;

    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        Load();
        ApplyVolumes(); // Ahora se aplican los valores cargados correctamente
        UpdateTexts();
    }

    public void CambiarVolumen()
    {
        ApplyVolumes();
        Save();
        UpdateTexts();
    }

    private void ApplyVolumes()
    {
        SetVolume("MasterVolume", sliderMaster.value);
        SetVolume("MusicVolume", sliderMusic.value);
        SetVolume("EffectsVolume", sliderEffects.value);
    }

    private void SetVolume(string parameterName, float value)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(parameterName, volumeDb);
    }

    private void Load()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float effects = PlayerPrefs.GetFloat("EffectsVolume", 1f);

        sliderMaster.value = master;
        sliderMusic.value = music;
        sliderEffects.value = effects;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", sliderMaster.value);
        PlayerPrefs.SetFloat("MusicVolume", sliderMusic.value);
        PlayerPrefs.SetFloat("EffectsVolume", sliderEffects.value);
    }

    private void UpdateTexts()
    {
        MasterVolume.text = Mathf.RoundToInt(sliderMaster.value * 100f) + "%";
        MusicVolume.text = Mathf.RoundToInt(sliderMusic.value * 100f) + "%";
        EffectsVolume.text = Mathf.RoundToInt(sliderEffects.value * 100f) + "%";
    }
}
