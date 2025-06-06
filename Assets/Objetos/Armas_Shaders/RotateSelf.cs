using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    [Tooltip("Velocidad de rotación en grados por segundo")]
    public float rotationSpeed = 50f;

    
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
