using UnityEngine;
public class Habitacion
{
    private float x1, x2, y1, y2;
    private float ancho, alto;
    public float numColumnas, numFilas;
    private Vector3 centro;

    public Habitacion(float x1, float x2, float y1, float y2)
    {
        this.x1 = x1; this.x2 = x2;
        this.y1 = y1; this.y2 = y2;
        ancho = x2 - x1;
        alto = y2 - y1;
        centro = new Vector3((x2 - x1) / 2 + x1, 0, (y2 - y1) / 2 + y1);
        numColumnas = Mathf.FloorToInt(ancho / 5f);
        numFilas = Mathf.FloorToInt(alto / 5f);
    }
}
