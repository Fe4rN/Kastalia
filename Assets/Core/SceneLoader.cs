using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    private Dictionary<string, Action> escenaCallbacks = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CargarEscena(string nombreEscena, Action accionPosterior = null)
    {
        if (accionPosterior != null)
        {
            escenaCallbacks[nombreEscena] = accionPosterior;
        }

        SceneManager.LoadScene(nombreEscena);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (escenaCallbacks.TryGetValue(scene.name, out var accion))
        {
            accion?.Invoke();
            escenaCallbacks.Remove(scene.name);
        }
    }
}
