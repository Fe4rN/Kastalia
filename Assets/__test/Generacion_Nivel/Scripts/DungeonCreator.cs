using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{

    [SerializeField] SalasNivel salasNivel;
    public List<Casilla> casillas = new List<Casilla>();

    List<float> columnas = new List<float>();
    List<float> filas = new List<float>();

    float lado;
    float ladoColumnas;
    float ladoFilas;

    private void Awake()
    {
        generarReticula();

        colocarSalas();

        conectarSalas();

        validarCaminos();
    }


    private void validarCaminos()
    {

    }

    private void conectarSalas()
    {

    }

    private void colocarSalas()
    {
        List<Casilla> casillasOcupadas = new List<Casilla>();
        for (int i = 0; i < salasNivel.numSalas; i++)
        {
            int selectIndex = Random.Range(0, casillas.Count);
            casillasOcupadas.Add(casillas[selectIndex]);
            casillas.RemoveAt(selectIndex);
        }

        RoomRenderer roomRenderer = GetComponent<RoomRenderer>();

        for (int i = 0; i < casillasOcupadas.Count; i++)
        {
            Vector3 centro = casillasOcupadas[i].getCentro();
            
            Room room = new Room(
                centro,
                salasNivel.anchuraMinSala, salasNivel.anchuraMaxSala,
                salasNivel.alturaMinSala, salasNivel.alturaMaxSala
            );
            
            roomRenderer.RenderRoom(room, centro);
        }
    }

    private void generarReticula()
    {

        lado = Mathf.Sqrt(salasNivel.numSalas);
        ladoColumnas = Mathf.Floor(lado);
        ladoFilas = Mathf.Ceil(lado);


        Debug.Log($"{ladoColumnas} x {ladoFilas}");

        #region --- Crear filas y columnas ---

        float anchuraTotal = 0;
        float altruraTotal = 0;

        columnas.Add(0f);
        filas.Add(0f);

        for (int i = 0; i < ladoColumnas; i++)
        {
            float anchura = Random.Range(salasNivel.anchuraMinSala, salasNivel.anchuraMaxSala) + salasNivel.espacioEntreSalas;
            if (i != 0 && i != ladoColumnas - 1) anchura += salasNivel.espacioEntreSalas;

            anchuraTotal += anchura;

            columnas.Add(anchuraTotal);
        }

        for (int j = 0; j < ladoFilas; j++)
        {
            float altura = Random.Range(salasNivel.alturaMinSala, salasNivel.alturaMaxSala) + salasNivel.espacioEntreSalas;
            if (j != 0 && j != ladoFilas - 1) altura += salasNivel.espacioEntreSalas;

            altruraTotal += altura;

            filas.Add(altruraTotal);
        }

        #endregion

        #region --- Centrar Rejilla ---
        float desplazamientoX = anchuraTotal / 2;
        float desplazamientoZ = altruraTotal / 2;

        for (int i = 0; i < columnas.Count; i++)
        {
            columnas[i] -= desplazamientoX;
        }

        for (int i = 0; i < filas.Count; i++)
        {
            filas[i] -= desplazamientoZ;
        }

        #endregion

        #region --- Crear Casillas ---
        for (int i = 0; i < columnas.Count - 1; i++)
        {
            for (int j = 0; j < filas.Count - 1; j++)
            {
                float inicioX = columnas[i];
                float finX = columnas[i + 1];
                float inicioY = filas[j];
                float finY = filas[j + 1];

                if (i > 0) inicioX += salasNivel.espacioEntreSalas;
                if (i < columnas.Count - 2) finX -= salasNivel.espacioEntreSalas;

                if (j > 0) inicioY += salasNivel.espacioEntreSalas;
                if (j < filas.Count - 2) finY -= salasNivel.espacioEntreSalas;

                Casilla casilla = new Casilla(inicioX, finX, inicioY, finY);
                casillas.Add(casilla);
            }
        }
        #endregion


    }


    private void Update()
    {
        for (int i = 0; i < columnas.Count; i++)
        {
            Vector3 inicio = new Vector3(columnas[i], 0, filas[0]);
            Vector3 fin = new Vector3(columnas[i], 0, filas[filas.Count - 1]);
            Debug.DrawLine(inicio, fin, Color.red);
        }

        for (int i = 0; i < filas.Count; i++)
        {
            Vector3 inicio = new Vector3(columnas[0], 0, filas[i]);
            Vector3 fin = new Vector3(columnas[columnas.Count - 1], 0, filas[i]);
            Debug.DrawLine(inicio, fin, Color.blue);
        }
    }
}

public struct Casilla
{
    float x1, x2, y1, y2;

    public Casilla(float x1, float x2, float y1, float y2)
    {
        this.x1 = x1; this.x2 = x2;
        this.y1 = y1; this.y2 = y2;
    }

    public Vector3 getCentro()
    {
        return new Vector3((x2 - x1) / 2 + x1, 0, (y2 - y1) / 2 + y1);
    }

    public Vector3 getSize()
    {
        return new Vector3(x2 - x1, 0, y2 - y1);
    }
}