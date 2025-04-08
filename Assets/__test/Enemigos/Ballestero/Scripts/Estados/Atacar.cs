using System;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{

    BallesteroController enemigo;
    NavMeshAgent agent;
    BallesteroController controller;

    public float arrowForce = 20f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
        enemigo = GetComponent<BallesteroController>();
    }

    void Update()
    {
        if (controller.jugador)
        {
            if(controller.distanciaAJugador < controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
            {
                shootArrow();
            }
        }
    }

    private void shootArrow()
    {
        GameObject arrow = Instantiate(controller.arrowPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Vector3 direction = (controller.jugador.position - transform.position).normalized;
        rb.AddForce(direction * arrowForce, ForceMode.Impulse);
    }
}
