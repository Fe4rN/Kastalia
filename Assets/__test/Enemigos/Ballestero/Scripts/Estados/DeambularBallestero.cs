using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DeambularEstadoBallestero : Estado
{
    private BallesteroController stats;
    private float radio = 5f;
    private Vector3 posicionAleatoria;
    NavMeshAgent agent;
    BallesteroController controller;
    private bool estaDeambulando = false;
    private bool isWondering = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
        stats = GetComponent<BallesteroController>();
        StartCoroutine(MustveBeenTheWind());
    }

    void Update()
    {
        // if (GameManager.instance.isPaused) return;
        if(isWondering) return;
        if (controller != null && controller.jugador != null)
        {
            Debug.Log("Dentro del Update de DeambularEstado");
            if (controller.distanciaAJugador <= stats.shootingDistance && controller.distanciaAJugador > stats.safeDistance)
            {
                Debug.Log("Atacando al jugador");
                StopCoroutine(DeambularCoorutina());
                estaDeambulando = false;
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.atacarEstado.Value);
            }
            else if (controller.distanciaAJugador <= stats.safeDistance)
            {
                Debug.Log("Manteniendo distancia del jugador");
                StopCoroutine(DeambularCoorutina());
                estaDeambulando = false;
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.mantenerDistanciaEstado.Value);
            }
            else
            {
                if (!estaDeambulando)
                {
                    Debug.Log("Deambulando");
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
        Debug.Log("Deambulando2");
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
                Debug.Log("Esperando " + tiempoEspera + " segundos antes de elegir una nueva posición.");
            }

            estaDeambulando = false;
        }
    }

    IEnumerator MustveBeenTheWind()
    {
        isWondering = true;
        float randomPause = Random.Range(3f, 5f);
        Vector3 lastKnownPlayerDirection = (controller.jugador.position - transform.position).normalized;

        float timer = 0f;
        while (timer < randomPause)
        {
            Vector3 lookPoint = transform.position + lastKnownPlayerDirection;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPoint - transform.position), Time.deltaTime * 2f);
            timer += Time.deltaTime;
            yield return null;
        }

        isWondering = false;
    }
}
