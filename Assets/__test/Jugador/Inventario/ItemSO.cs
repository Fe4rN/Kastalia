using UnityEngine;

[CreateAssetMenu(fileName = "Arma", menuName = "Equipables/Arma")]
public class ItemSO : ScriptableObject
{
    [Header("Propiedades del item")]
    public string nombre;
    public float daño;
    public Sprite icono;
}

public enum TipoItem { Espada, Arco };
