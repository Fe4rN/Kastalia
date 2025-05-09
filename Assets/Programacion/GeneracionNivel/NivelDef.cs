using UnityEngine;

[CreateAssetMenu(fileName = "Sala #", menuName = "Generación de Mazmorras/Nueva Sala")]
public class NivelDef : ScriptableObject
{
    public int numSalas = 10;
    public float anchuraMinSala = 5f;
    public float anchuraMaxSala = 10f;
    public float alturaMinSala = 5f;
    public float alturaMaxSala = 10f;
    public float anchuraPared = .5f;
    public float anchuraPasillo = 3f;
    public float espacioEntreSalas { get => anchuraPasillo + (anchuraPared * 4); }

}
