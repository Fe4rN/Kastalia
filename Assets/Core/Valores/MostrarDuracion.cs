using UnityEngine;
using TMPro;

public class MostrarDuracion : MonoBehaviour
{
    private TextMeshProUGUI texto;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        ActualizarTexto();
    }

    public void ActualizarTexto()
    {
        if (PlayerPrefs.HasKey("DuracionPartida"))
        {
            string valor = PlayerPrefs.GetString("DuracionPartida");
            texto.text = "Duración: " + valor;
            Debug.Log("[MostrarDuracion] Texto actualizado: " + valor);
        }
        else
        {
            texto.text = "Duración: --:--";
            Debug.Log("[MostrarDuracion] No se encontró DuracionPartida en PlayerPrefs.");
        }
    }
}
