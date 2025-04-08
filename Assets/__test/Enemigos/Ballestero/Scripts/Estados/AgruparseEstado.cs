using UnityEngine;
using UnityEngine.AI;

public class AgruparseEstado : Estado
{
    NavMeshAgent agent;
    BallesteroController controller;
    public float squadRadius = 10f;
    public float groupCloseEnoughDistance = 2f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = maquina as BallesteroController;
    }

    void Update()
    {
        if (controller == null) return;

        Collider[] allies = Physics.OverlapSphere(transform.position, squadRadius);
        Vector3 groupCenter = Vector3.zero;
        int count = 0;

        foreach (Collider col in allies)
        {
            if (col.gameObject != this.gameObject && col.CompareTag("Enemy"))
            {
                groupCenter += col.transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            groupCenter /= count;
            agent.SetDestination(groupCenter);

            if (Vector3.Distance(transform.position, groupCenter) <= groupCloseEnoughDistance)
            {
                controller.SetEstado(controller.deambularEstado.Value); // Or atacarEstado if player nearby
            }
        }
        else
        {
            // No allies found, fallback
            controller.SetEstado(controller.deambularEstado.Value);
        }
    }
}
