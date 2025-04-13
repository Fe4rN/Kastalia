using System.Collections;

using UnityEngine;


public class ArmaAutoDestructora : MonoBehaviour
{
    public enum PersonajeCompatible { Lyx, Dreven }

    [SerializeField] private PersonajeCompatible personajeRequerido;

    void Start()
    {
        // Espera un momento para asegurarte que GameManager y el personaje están listos
        StartCoroutine(EsperarYVerificar());
    }

    private IEnumerator EsperarYVerificar()
    {
        // Esperamos unos frames por si la asignación tarda
        yield return new WaitForSeconds(0.2f);

        // Si no hay personaje seleccionado aún, salimos
        if (GameManager.instance == null || GameManager.instance.personajeSeleccionado == null)
        {
            yield break;
        }

        GameObject personaje = GameManager.instance.personajeSeleccionado;

        // Verifica si el personaje coincide con el esperado
        if ((personajeRequerido == PersonajeCompatible.Lyx && personaje != GameManager.instance.Lyx) ||
            (personajeRequerido == PersonajeCompatible.Dreven && personaje != GameManager.instance.Dreven))
        {
            Destroy(gameObject); // 🚫 No es el personaje correcto, destruir arma
        }
    }
}
