using UnityEngine;

public class ObstaculoTransparente : MonoBehaviour
{
    [SerializeField] Material transparencia;

    Material opaco;

    Renderer rend;

    public bool hitted = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        opaco = rend.material;
    }

    private void Update()
    {
        if(hitted)
        {
            if(rend.material != transparencia) rend.material = transparencia;
            hitted = false;
        } 
        else
        {
            if (rend.material != opaco) rend.material = opaco;
        }
    }
}
