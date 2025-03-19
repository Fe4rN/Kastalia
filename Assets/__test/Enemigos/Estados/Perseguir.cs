using UnityEngine;
using UnityEngine.AI;

public class Perseguir : Estado
{
    NavMeshAgent agent;
    Transform player;
    Enemigo controller;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as Enemigo;
        player = controller.Player;
    }

    void Update()
    {
        if(agent == null) {return;}
        if (player == null) return;
        float distanciaAJugador = Vector3.Distance(agent.transform.position, player.position);
        if (distanciaAJugador <= controller.DetectionDistance)
        {
            agent.SetDestination(player.position);
            agent.transform.LookAt(new Vector3(player.position.x, agent.transform.position.y, player.position.z));
        } else {
            controller.SetEstado(controller.deambularEstado.Value);
            return;
        }
    }
}
