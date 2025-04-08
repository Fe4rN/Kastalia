using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{

    BallesteroController enemigo;
    NavMeshAgent agent;
    BallesteroController controller;

    public float arrowForce = 20f;
    private bool isFiring = false;

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
            if (controller.distanciaAJugador < controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
            {
                if (!isFiring)
                {
                    isFiring = true;
                    StartCoroutine(ShootArrow());
                }
            } else if (controller.distanciaAJugador < controller.safeDistance)
            {
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.mantenerDistanciaEstado.Value);
            } else if (controller.distanciaAJugador > controller.shootingDistance)
            {
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.deambularEstado.Value);
            }


        }
    }

    private IEnumerator ShootArrow()
    {
        Debug.Log("Disparando flecha al jugador");
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 2f;
        GameObject arrow = Instantiate(controller.arrowPrefab, spawnPos, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Vector3 direction = (controller.jugador.position - transform.position).normalized;
        rb.AddForce(direction * arrowForce, ForceMode.Impulse);

        yield return new WaitForSeconds(controller.fireCooldown);
        isFiring = false;
    }
}
