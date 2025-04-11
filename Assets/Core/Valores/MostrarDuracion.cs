using UnityEngine;
using TMPro;

public class MostrarDuracion : MonoBehaviour
{
    void Start()
    {
        TextMeshProUGUI texto = GetComponent<TextMeshProUGUI>();

        // Recupera el tiempo guardado al final del juego
        if (PlayerPrefs.HasKey("DuracionPartida"))
        {
            texto.text = "Duración: " + PlayerPrefs.GetString("DuracionPartida");
        }
        
    }
}
