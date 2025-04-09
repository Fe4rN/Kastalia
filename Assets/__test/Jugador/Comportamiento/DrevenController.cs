using UnityEngine;

public class DrevenController : MonoBehaviour
{
    void Update(){
        if (Input.GetMouseButtonDown(0)) arco.DisparoLigero();
        if (Input.GetMouseButton(1)) arco.EmpezarCarga();
        if (Input.GetMouseButtonUp(1)) arco.SoltarCarga();
    }
}
