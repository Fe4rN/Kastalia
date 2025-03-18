using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public abstract class Maquina : MonoBehaviour
{

    public UnityEvent<string> OnStateChanged;

    Estado[] estados;

    Estado _estado;

    public Estado Estado
    {
        get => _estado;
        set
        {
            if (_estado == value) return;
            _estado = value;
            foreach (Estado estado in estados)
            {
                estado.enabled = (estado == _estado);
            }
            OnStateChanged.Invoke(_estado.Nombre);
        }
    }

    void Awake()
    {
        estados = GetComponents<Estado>();
        foreach (Estado estado in estados)
        {
            if (estado.Inicial) Estado = estado;
        }
        if (Estado == null) Estado = estados[0];
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public void SetEstado(string nombre)
    {
        foreach (Estado estado in estados)
        {
            if (estado.Nombre == nombre)
            {
                Estado = estado;
                return;
            }
        }
        Debug.LogError($"No hay un estado {nombre}");
    }
}