using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Deambular : Estado
{
    private Enemigo stats;
    private float radio = 30f;
    private Vector3 posicionAleatoria;
    private Coroutine rutinaActual;

    private NavMeshAgent agent;
    private Enemigo controller;
    private bool estaDeambulando = false;

    protected override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>(); 
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent no está asignado en el GameObject.");
        }

        controller = maquina as Enemigo;
        stats = GetComponent<Enemigo>();
    }

    protected override void OnEnter()
    {
        Debug.Log("OnEnter llamado en Deambular");

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent no asignado");
            return;
        }

        agent.isStopped = false;

        if (rutinaActual != null) StopCoroutine(rutinaActual);
        rutinaActual = StartCoroutine(DeambularCoorutina());
        agent.isStopped = true; 

    }

    protected override void OnExit()
    {
        if (rutinaActual != null) StopCoroutine(rutinaActual);
        rutinaActual = null;
        agent.ResetPath();
        estaDeambulando = false;
    }

    protected override void OnUpdate()
    {
        if (GameManager.instance != null && GameManager.instance.isPaused) return;

        if (controller != null && controller.Player != null)
        {
            float distanciaAJugador = Vector3.Distance(transform.position, controller.Player.position);
            if (distanciaAJugador <= stats.DetectionDistance)
            {
                if (rutinaActual != null) StopCoroutine(rutinaActual);
                rutinaActual = null;
                estaDeambulando = false;
                maquina.SetEstado(controller.perseguirEstado.Value);

            }
        }
    }

    private Vector3 ElegirPosicionAleatoria()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 direccion2D = Random.insideUnitCircle * radio;
            Vector3 punto = transform.position + new Vector3(direccion2D.x, 0, direccion2D.y);

            if (NavMesh.SamplePosition(punto, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position; 
    }

    IEnumerator DeambularCoorutina()
    {
        estaDeambulando = true;

        while (true)
        {
            // Espera antes de elegir un punto (mirar)
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            posicionAleatoria = ElegirPosicionAleatoria();
            agent.SetDestination(posicionAleatoria);
            transform.LookAt(posicionAleatoria);

            while (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                yield return null;
            }

          
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }
}