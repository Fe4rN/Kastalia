using System.Collections;
using UnityEngine;

public class Arquero : MonoBehaviour
{
    public GameObject prefabFlecha;
    public Transform puntoDisparo;
    public float fuerza = 30f;
    public float daño = 10f;
    public float delayDisparo = 0.1f;
    public float delayRafaga = 0.05f;

    private float cooldownLigero = 0.5f;
    private float cooldownCargado = 1f;
    private float tiempoCarga = 1f;

    private float acumuladoCarga = 0f;
    private bool cargando = false;
    private bool puedeDisparar = true;

    public void DisparoLigero()
    {
        if (!puedeDisparar) return;
        StartCoroutine(Disparar(1, cooldownLigero, delayDisparo));
    }

    public void EmpezarCarga()
    {
        acumuladoCarga += Time.deltaTime;
        if (acumuladoCarga >= tiempoCarga && !cargando)
        {
            cargando = true;
            StartCoroutine(Disparar(5, cooldownCargado, delayRafaga));
        }
    }

    public void SoltarCarga()
    {
        acumuladoCarga = 0f;
        cargando = false;
    }

    IEnumerator Disparar(int cantidad, float cooldown, float delay)
    {
        puedeDisparar = false;

        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(delay);

            // Desviación en abanico para ráfaga
            Quaternion rotacion = puntoDisparo.rotation;
            if (cantidad > 1)
            {
                float spread = 5f; // grados de separación entre flechas
                float offset = (i - (cantidad - 1) / 2f) * spread;
                rotacion = puntoDisparo.rotation * Quaternion.Euler(0, offset, 0);
            }

            GameObject flecha = Instantiate(prefabFlecha, puntoDisparo.position, rotacion);
            Rigidbody rb = flecha.GetComponent<Rigidbody>();
            rb.AddForce(rotacion * Vector3.forward * fuerza, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(cooldown);
        puedeDisparar = true;
    }
}
