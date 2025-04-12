// EnemyHighlighter.cs
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EnemyHighlighter : MonoBehaviour
{
    [Header("Highlight Settings")]
    [SerializeField] private Color highlightColor = Color.red;
    [SerializeField] private float outlineWidth = 0.1f;
    
    private Material originalMaterial;
    private Material outlineMaterial;
    private Renderer enemyRenderer;
    
    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        originalMaterial = enemyRenderer.material;
        
        // Create outline material
        outlineMaterial = new Material(Shader.Find("Custom/OutlineShader"));
        outlineMaterial.SetColor("_Color", originalMaterial.color);
        outlineMaterial.SetTexture("_MainTex", originalMaterial.mainTexture);
        outlineMaterial.SetColor("_OutlineColor", highlightColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
    }
    
    private void OnMouseEnter()
{
    
    Material[] materials = enemyRenderer.materials;

   
    if (materials.Length == 1)
    {
        Material[] newMaterials = new Material[2];
        newMaterials[0] = materials[0];          
        newMaterials[1] = outlineMaterial;      
        enemyRenderer.materials = newMaterials;
    }
}

private void OnMouseExit()
{
    
    Material[] materials = enemyRenderer.materials;

    if (materials.Length == 2)
    {
        Material[] newMaterials = new Material[1];
        newMaterials[0] = materials[0];          
        enemyRenderer.materials = newMaterials;
    }
}
    
    private void OnDestroy()
    {
        if (outlineMaterial != null)
        {
            Destroy(outlineMaterial);
        }
    }
}