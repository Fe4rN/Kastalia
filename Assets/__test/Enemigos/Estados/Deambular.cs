using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Deambular : Estado
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
        if (Menu_Pausa.JuegoPausado) return;

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
        Vector2 direccionAleatoria = Random.insideUnitCircle * radio;
        return agent.transform.position + new Vector3(direccionAleatoria.x, 0, direccionAleatoria.y);
    }

    IEnumerator DeambularCoorutina()
    {
        if (agent == null) yield break;
        if (!estaDeambulando)
        {
            estaDeambulando = true;
            posicionAleatoria = ElegirPosicionAleatoria();

            while (posicionAleatoria != null && (Vector3.Distance(agent.transform.position, posicionAleatoria) > .5f))
            {
                while (Menu_Pausa.JuegoPausado) yield return null;

                agent.SetDestination(posicionAleatoria);
                agent.transform.LookAt(posicionAleatoria);

                while (agent.pathPending || (agent.isOnNavMesh && agent.remainingDistance > .2f))
                {
                    if (Menu_Pausa.JuegoPausado) yield return null;
                    yield return null;
                }

                float tiempoEspera = Random.Range(1f, 4f);
                while (Menu_Pausa.JuegoPausado) yield return null;
                yield return new WaitForSeconds(tiempoEspera);
            }

            estaDeambulando = false;
        }
    }
}
