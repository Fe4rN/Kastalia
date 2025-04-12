using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{
    CaballeroNormalController controller;
    Transform player;
    NavMeshAgent agent;

    public int damage = 1;
    public EnemigoTipo tipoDeEnemigo;

    [SerializeField] float distanceToPlayer;

    private Coroutine ataqueCorriendo;

    void Start()
    {
        controller = maquina as CaballeroNormalController;
        player = controller.Player;
        agent = GetComponent<NavMeshAgent>();
        agent.ResetPath();
    }

    private void Update()
    {
        if (player == null)
        {
            // Detener ataque si el jugador desaparece
            if (ataqueCorriendo != null)
            {
                StopCoroutine(ataqueCorriendo);
                ataqueCorriendo = null;
            }
            return;
        }

        DistanceToPlayer();

        if (distanceToPlayer <= controller.distanciaAtaque)
        {
            if (ataqueCorriendo == null)
            {
                Debug.Log("[Atacar] Iniciando ataque al jugador");
                ataqueCorriendo = StartCoroutine(LoopAtaque());
            }
        }
        else
        {
            if (ataqueCorriendo != null)
            {
                Debug.Log("[Atacar] Enemigo deja de atacar al jugador");
                StopCoroutine(ataqueCorriendo);
                ataqueCorriendo = null;
                maquina.SetEstado(controller.perseguirEstado.Value);
            }
        }
    }

    IEnumerator LoopAtaque()
    {
        while (true)
        {
            if (player != null)
            {
                // Rotar hacia el jugador
                Vector3 direccion = player.position - transform.position;
                direccion.y = 0;
                if (direccion != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(direccion);

                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("[Atacar] Daño aplicado: " + controller.attackDamage);
                    playerHealth.TakeDamage(controller.attackDamage);
                }
            }
            else
            {
                yield break; // Salir si el jugador desaparece
            }

            yield return new WaitForSeconds(1f); // Tiempo entre ataques
        }
    }

    private void DistanceToPlayer()
    {
        if (player == null) return;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }
}
