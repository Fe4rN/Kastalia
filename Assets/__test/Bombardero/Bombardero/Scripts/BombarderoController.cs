using System.Collections;
using UnityEngine;

public class BombarderoController : Maquina
{
    //Atributos relacionados al enemigo
    public float shootingDistance = 15f;
    public float safeDistance = 10f;
    public float fireCooldown = 6f;
    public bool isFiring = false;

    //Atributos relacionados al jugador
    public Transform jugador;
    public float distanciaAJugador;
    
    //Estados
    public NombreEstado deambularEstado;
    public NombreEstado mantenerDistanciaEstado;
    public NombreEstado atacarEstado;
    public Bomba bombaPrefab;

    void Start()
    {
        if (FindFirstObjectByType<CharacterController>() == null) { return; }
        else { jugador = FindFirstObjectByType<CharacterController>().transform; }
    }

    void Update()
    {
        if (!jugador)
        {
            if (FindFirstObjectByType<CharacterController>() == null) return;
            else{ jugador = FindFirstObjectByType<CharacterController>().transform; }
        }
        distanciaAJugador = getDistanceToPlayer();
    }

    private float getDistanceToPlayer(){
        if (jugador){
            return Vector3.Distance(transform.position, jugador.position);
        } else {
            Debug.LogError("No se ha encontrado al jugador.");
            return 0;
        }
    }

    public IEnumerator ShootBomba()
    {
        Vector3 spawnPos = transform.position + transform.forward * 2f + Vector3.up * 1.75f;
        Vector3 targetPos = jugador.position;
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
        Bomba bomba = Instantiate(bombaPrefab, spawnPos, Quaternion.LookRotation(direction));
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

        yield return new WaitForSeconds(fireCooldown);
        isFiring = false;
    }
}
