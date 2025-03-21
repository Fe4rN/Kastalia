using UnityEngine;

public abstract class Habilidad : MonoBehaviour
{
    public string Name { get; private set; }
    public AbilityType Type { get; private set; }

    public Habilidad(string name, AbilityType type)
    {
        Name = name;
        Type = type;
    }

    private float damage;
    private float damageRadius;
    private float killCountCooldown;
    private string nombre;
    private string type;
}
