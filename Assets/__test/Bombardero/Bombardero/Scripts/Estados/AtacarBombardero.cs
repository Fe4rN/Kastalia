using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AtacarBombardero : Estado
{

    NavMeshAgent agent;
    BombarderoController controller;

    public float bombaForce = 8f;
    private bool isFiring = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BombarderoController;
        controller = GetComponent<BombarderoController>();
    }

    void Update()
    {
        transform.LookAt(controller.jugador);
        if (controller.jugador)
        {
            if (controller.distanciaAJugador < controller.shootingDistance && controller.distanciaAJugador > controller.safeDistance)
            {
                if (!isFiring)
                {
                    isFiring = true;
                    StartCoroutine(ShootBomba());
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

    private IEnumerator ShootBomba()
    {
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 targetPos = controller.jugador.position;
        Vector3 direction = (targetPos - spawnPos).normalized;
        
        // Physics calculations for arc
        float gravity = Physics.gravity.magnitude;
        float angle = 45f * Mathf.Deg2Rad; // Launch angle in radians
        float horizontalDistance = Vector3.Distance(
            new Vector3(spawnPos.x, 0, spawnPos.z),
            new Vector3(targetPos.x, 0, targetPos.z)
        );
        
        // Calculate initial velocity for parabolic arc
        float initialVelocity = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * angle));
        
        // Create bomba with proper rotation
        GameObject bomba = Instantiate(controller.bombaPrefab, spawnPos, Quaternion.LookRotation(direction));
        Rigidbody rb = bomba.GetComponent<Rigidbody>();
        
        // Calculate firing direction (combining horizontal and vertical components)
        Vector3 firingDirection = new Vector3(
            direction.x * Mathf.Cos(angle),
            Mathf.Sin(angle),
            direction.z * Mathf.Cos(angle)
        ).normalized;

        // Apply force
        rb.AddForce(firingDirection * initialVelocity, ForceMode.VelocityChange);
        
        // Add realistic bomba rotation (fixed version)
        if (Random.value > 0.5f) // 50% chance to add rotation
        {
            rb.AddTorque(new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(-1f, 1f),
                Random.Range(-0.5f, 0.5f)
            ));
        }

        yield return new WaitForSeconds(controller.fireCooldown);
        isFiring = false;
    }

}
