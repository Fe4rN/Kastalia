using TMPro;
using UnityEngine;

public class Notificacion : MonoBehaviour
{
    [SerializeField] private TMP_Text textoNotificacion;

    Animator animator;

    void Start()
    {

    }

    public void EstablecerNombreObjeto(string nombre)
    {
        textoNotificacion.text = "¡HAS OBTENIDO: " + nombre + "!";
    }
    
    public void DesactivarNotificacion()
    {
        this.gameObject.SetActive(false);
    }
}
