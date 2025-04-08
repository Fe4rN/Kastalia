using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DeambularEstado : Estado
{
    private Enemigo stats;
    private float radio = 5f;
    private Vector3 posicionAleatoria;
    NavMeshAgent agent;
    Enemigo controller;
    private bool estaDeambulando = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as Enemigo;
        stats = GetComponent<Enemigo>();
    }

    void Update()
    {
        if (GameManager.instance.isPaused) return;

        if (controller != null && controller.Player != null)
        {
            float distanciaAJugador = Vector3.Distance(agent.transform.position, controller.Player.position);
            if (distanciaAJugador <= stats.DetectionDistance && estaDeambulando)
            {
                StopCoroutine(DeambularCoorutina());
                estaDeambulando = false;
                controller.SetEstado(controller.perseguirEstado.Value);
            }
            else
            {
                if (!estaDeambulando)
                {
                    StartCoroutine(DeambularCoorutina());
                }
            }
        }
    }

    private Vector3 ElegirPosicionAleatoria()
{
    Vector3 randomDirection = Random.insideUnitSphere * radio;
    randomDirection.y = 0;
    Vector3 targetPosition = agent.transform.position + randomDirection;

    NavMeshHit hit;
    if (NavMesh.SamplePosition(targetPosition, out hit, radio, NavMesh.AllAreas))
    {
        return hit.position;
    }
    else
    {
        return agent.transform.position;
    }
}


    IEnumerator DeambularCoorutina()
    {
        if (agent == null) yield break;
        if (!estaDeambulando)
        {
            estaDeambulando = true;
            posicionAleatoria = ElegirPosicionAleatoria();

            while (posicionAleatoria != null && Vector3.Distance(agent.transform.position, posicionAleatoria) > .5f)
            {

                agent.SetDestination(posicionAleatoria);
                agent.transform.LookAt(posicionAleatoria);

                while (agent.pathPending || (agent.isOnNavMesh && agent.remainingDistance > .2f))
                {
                    yield return null;
                }

                float tiempoEspera = Random.Range(1f, 4f);
                yield return new WaitForSeconds(tiempoEspera);
            }

            estaDeambulando = false;
        }
    }
}
