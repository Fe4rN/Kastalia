using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MantenerDistanciaBombardero : Estado
{
    NavMeshAgent agent;
    BombarderoController controller;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
    }

    void Update()
    {
        if (GameManager.instance.isPaused) return;

        if (controller != null && controller.jugador != null)
        {
            float distancia = controller.distanciaAJugador;
            Vector3 directionToPlayer = (controller.jugador.position - transform.position).normalized;

            if (distancia < controller.safeDistance)
            {
                animator.SetBool("IsWandering", true);
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                if (!controller.isFiring)
                {
                    controller.isFiring = true;
                    StartCoroutine(controller.ShootBomba());
                }
                Vector3 fleePosition = transform.position - directionToPlayer * 2f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(fleePosition, out hit, 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            else if (distancia <= controller.shootingDistance)
            {
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                animator.SetBool("IsWandering", false);
                controller.SetEstado(controller.atacarEstado.Value);
            }
            else
            {
                animator.SetBool("IsWandering", true);
                agent.SetDestination(controller.jugador.position);
            }
        }
    }
}
