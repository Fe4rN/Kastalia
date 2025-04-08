using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{

    NavMeshAgent agent;
    BallesteroController controller;

    public float arrowForce = 20f;
    private bool isFiring = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
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
            }
            else if (controller.distanciaAJugador < controller.safeDistance)
            {
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.mantenerDistanciaEstado.Value);
            }
            else if (controller.distanciaAJugador > controller.shootingDistance)
            {
                agent.ResetPath();
                transform.LookAt(controller.jugador);
                controller.SetEstado(controller.deambularEstado.Value);
            }


        }
    }

    private IEnumerator ShootArrow()
    {
        Vector3 spawnPos = transform.position + transform.forward * 0.5f + Vector3.up * 1.75f;
        Vector3 direction = (controller.jugador.position - transform.position).normalized;

        GameObject arrow = Instantiate(controller.arrowPrefab, spawnPos, Quaternion.LookRotation(direction));
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(direction * arrowForce, ForceMode.Impulse);

        yield return new WaitForSeconds(controller.fireCooldown);
        isFiring = false;
    }

}
