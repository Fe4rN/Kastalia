using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{
    CaballeroNormalController controller;
    Transform player;
    NavMeshAgent agent;
    bool puedeAtacar = true;

    public float damage = 20f;
    public EnemigoTipo tipoDeEnemigo;

    [SerializeField] float distanceToPlayer;

    void Start()
    {
        player = FindFirstObjectByType<Acciones>().transform;
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as CaballeroNormalController;
        agent.ResetPath();
    }

    private void Update()
    {
        if (player == null) return;
        DistanceToPlayer();

        // Check if within visual attack range and can attack
        if (distanceToPlayer <= controller.distanciaAtaque && puedeAtacar )
        {
            puedeAtacar = false;
            StartCoroutine(LoopAtaque());
        }

        if (distanceToPlayer > controller.AttackDistance)
        {
            StopAllCoroutines();
            maquina.SetEstado(controller.perseguirEstado.Value);
        }
        else
        {
            // Orient towards the player on the x-z plane
            Vector3 lookPos = new Vector3(player.position.x, 0, player.position.z);
            transform.LookAt(lookPos);
        }
    }


    IEnumerator LoopAtaque()
    {
        if (player != null)
        {
            Acciones acciones = player.GetComponent<Acciones>();
            if (acciones != null)
            {
                acciones.takeDamage(controller.attackDamage);
                Debug.Log("Enemigo ataca al jugador");
            }

            yield return new WaitForSeconds(1f); // Cooldown
            puedeAtacar = true;
        }
    }

    private void DistanceToPlayer()
    {
        if (player == null) return;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }
}
