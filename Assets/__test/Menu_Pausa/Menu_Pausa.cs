using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Pausa : MonoBehaviour
{
    public bool pause;
    public GameObject menuPausa;
    public static bool JuegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        pause = !pause;
        menuPausa.SetActive(pause);
        JuegoPausado = pause;
        Time.timeScale = pause ? 0 : 1;
        ToggleEnemyScripts(!pause);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    void ToggleEnemyScripts(bool state)
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemigo in enemigos)
        {
            // Desactivar comportamiento de enemigos
            Enemigo script = enemigo.GetComponent<Enemigo>();
            if (script != null)
            {
                script.enabled = state;
            }

            // Desactivar el comportamiento de deambular
            Deambular deambularScript = enemigo.GetComponent<Deambular>();
            if (deambularScript != null)
            {
                deambularScript.enabled = state;
            }

            // Desactivar el comportamiento de perseguir
            Perseguir perseguirScript = enemigo.GetComponent<Perseguir>();
            if (perseguirScript != null)
            {
                perseguirScript.enabled = state;
            }

            // Desactivar NavMeshAgent solo si está en el NavMesh
            UnityEngine.AI.NavMeshAgent agente = enemigo.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agente != null)
            {
                if (agente.isOnNavMesh) 
                {
                    agente.isStopped = !state;
                }
                agente.enabled = state; 
            }

            // Desactivar físicas del Rigidbody
            Rigidbody rb = enemigo.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = !state;
                if (!state) rb.linearVelocity = Vector3.zero; 
            }

            // Detener todas las corutinas activas solo si está pausado
            if (!state)
            {
                MonoBehaviour[] scripts = enemigo.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour s in scripts)
                {
                    if (s != null) s.StopAllCoroutines();
                }
            }
        }
    }

}
