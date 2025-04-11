using UnityEngine;

public class Cronometro : MonoBehaviour
{
    public static Cronometro instance;

    private float tiempo = 0f;
    private bool contando = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (contando)
        {
            tiempo += Time.deltaTime;
        }
    }

    public void Detener()
    {
        contando = false;
        // Guardamos el tiempo cuando se detiene
        PlayerPrefs.SetString("DuracionPartida", ObtenerTiempoFormateado());
        PlayerPrefs.Save();  // Aseguramos que el tiempo se guarda en PlayerPrefs
    }

    public void ReiniciarCronometro()
    {
        tiempo = 0f;  // Reinicia el contador de tiempo
        contando = true;  // Reinicia el conteo
    }

    public string ObtenerTiempoFormateado()
    {
        int minutos = Mathf.FloorToInt(tiempo / 60f);
        int segundos = Mathf.FloorToInt(tiempo % 60f);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public float ObtenerTiempoRaw()
    {
        return tiempo;
    }
}
