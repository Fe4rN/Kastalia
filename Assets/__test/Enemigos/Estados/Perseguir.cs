using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Perseguir : Estado
{
    NavMeshAgent agent;
    Transform player;
    Enemigo controller;
    bool esperando = false;

    protected override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<Enemigo>();
    }

    protected override void OnEnter()
    {
        if (controller != null)
            player = controller.Player;
        esperando = false;

        if (agent != null)
            agent.isStopped = false;
    }

    protected override void OnExit()
    {
        StopAllCoroutines();
        agent.ResetPath();
        esperando = false;
    }

    protected override void OnUpdate()
    {
        if (GameManager.instance != null && GameManager.instance.isPaused) return;
        if (controller == null || player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);
        Debug.Log($"[Perseguir] Distancia al jugador: {distancia}");

        if (distancia <= controller.AttackDistance)
        {
            Debug.Log("[Perseguir] Cambiando a estado atacar");
            maquina.SetEstado(controller.atacarEstado.Value);
            return;
        }

        if (distancia <= controller.DetectionDistance)
        {

            Debug.Log("[Perseguir] Persiguiendo al jugador");
            agent.SetDestination(player.position);

            Vector3 direccion = player.position - transform.position;
            direccion.y = 0;
            if (direccion != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direccion);


        }
        else if (!esperando)
        {
            Debug.Log("[Perseguir] Jugador fuera de rango. Esperando...");
            StartCoroutine(EsperarYVolverADeambular());
        }
    }

    IEnumerator EsperarYVolverADeambular()
    {
        esperando = true;
        agent.ResetPath();

        // Se queda mirando hacia la última posición del jugador
        if (player != null)
        {
            Vector3 direccion = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(direccion);
        }

        yield return new WaitForSeconds(2f);

        controller.SetEstado(controller.deambularEstado.Value);
    }
}