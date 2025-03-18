using UnityEngine;

public abstract class Estado : MonoBehaviour
{
    [SerializeField] NombreEstado nombre;

    [SerializeField]
    bool inicial = false;

    protected Maquina maquina;

    public string Nombre
    {
        get
        {
            if (!nombre) return "";
            return nombre.Value;
        }
    }
    public bool Inicial { get => inicial; set => inicial = value; }

    void Awake()
    {
        maquina = GetComponent<Maquina>();
        OnAwake();
    }

    protected virtual void OnAwake() { }
}
