using UnityEngine;

public class ObstaculoTransparente : MonoBehaviour
{
    [SerializeField] UnityEngine.Material transparencia;
    private Material[] opacoMaterials;

    Renderer rend;

    public bool hitted = false;
    void Start()
    {
        rend = GetComponent<Renderer>();
        opacoMaterials = rend.materials;
    }

    void Update()
    {
        if (hitted)
        {
            if (!AllMaterialsAre(rend.materials, transparencia))
            {
                Material[] newMats = new Material[rend.materials.Length];
                for (int i = 0; i < newMats.Length; i++)
                {
                    newMats[i] = transparencia;
                }
                rend.materials = newMats;
            }
            hitted = false;
        }
        else
        {
            if (!AllMaterialsAre(rend.materials, opacoMaterials[0]))
            {
                rend.materials = opacoMaterials;
            }
        }
    }

    private bool AllMaterialsAre(Material[] mats, Material mat)
    {
        foreach (var m in mats)
        {
            if (m != mat) return false;
        }
        return true;
    }

}

