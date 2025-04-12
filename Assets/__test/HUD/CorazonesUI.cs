using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CorazonesUI : MonoBehaviour
{
    public List<Image> listaCorazones;
    public GameObject corazonPrefab;
    public PlayerHealth vidaJugador;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    private void Awake()
    {
        vidaJugador.cambioVida.AddListener(CambiarCorazones);
    }

    private void CambiarCorazones(int vidaActual)
    {
        if (!listaCorazones.Any())
        {
            CrearCorazones(vidaJugador.vidaMaxima); // usamos vida máxima aquí
        }

        ActualizarCorazones(vidaActual);
    }

    private void CrearCorazones(int cantidadMaximaVida)
    {
        for (int i = 0; i < cantidadMaximaVida; i++)
        {
            GameObject corazon = Instantiate(corazonPrefab, transform);
            listaCorazones.Add(corazon.GetComponent<Image>());
        }
    }

    private void ActualizarCorazones(int vidaActual)
    {
        for (int i = 0; i < listaCorazones.Count; i++)
        {
            if (i < vidaActual)
            {
                listaCorazones[i].sprite = corazonLleno;
            }
            else
            {
                listaCorazones[i].sprite = corazonVacio;
            }
        }
    }
}
