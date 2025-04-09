using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class Atacar : Estado
{
    Enemigo controller;
    Transform player;
    NavMeshAgent agent;
    Enemigo stats;
    bool puedeAtacar = true;

    public float damage = 10f;
    public EnemigoTipo tipoDeEnemigo;

    [SerializeField] float radioAtaqueVisual;

    void Start()
    {
        player = FindFirstObjectByType<Acciones>().transform;
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as Enemigo;
        stats = GetComponent<Enemigo>();
    }

    private void Update()
    {
        DistanceToPlayer();
        if (radioAtaqueVisual <= controller.distanciaAtaque)
        {
            if (!puedeAtacar)
            {
                puedeAtacar = true;
                StartCoroutine(LoopAtaque());
            }
        }
        else
        {
            puedeAtacar = false;
        }
    }
    protected override void OnAwake()
    {
        controller = GetComponent<Enemigo>();
        agent = GetComponent<NavMeshAgent>();


    }
    /*
    protected override void OnEnter()
    {
        player = controller.Player;

        agent.ResetPath();
        StartCoroutine(LoopAtaque());
    }
    */
    protected override void OnExit()
    {

        StopAllCoroutines();
    }

    protected override void OnUpdate()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);

        if (distancia > controller.AttackDistance)
        {
            maquina.SetEstado(controller.perseguirEstado.Value); 
        }
        else
        {
           
            Vector3 lookPos = new Vector3(player.position.x, 0 , player.position.z);
            transform.LookAt(lookPos);
        }
    }

    IEnumerator LoopAtaque()
    {
        if (player != null && puedeAtacar)
        {
            Acciones acciones = player.GetComponent<Acciones>();
            if (acciones != null)
            {
                acciones.takeDamage(controller.attackDamage);
                Debug.Log("Enemigo ataca al jugador");
            }

            yield return new WaitForSeconds(8f); // Cooldown
            puedeAtacar = true;
        }
    }

    private void DistanceToPlayer()
    {
        if (player == null) return;
        radioAtaqueVisual = Vector3.Distance(transform.position, player.position);
        // Puedes usar 'distancia' si lo necesitas aquí
    }
}
