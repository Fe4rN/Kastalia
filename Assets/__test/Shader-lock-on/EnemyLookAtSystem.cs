using UnityEngine;

public class EnemyLookAtSystem : MonoBehaviour
{
    public float detectionRange = 15f;
    public LayerMask enemyMask;
    private Transform currentTarget;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }
    }

    void Update()
    {
        if (GameManager.instance.isPaused || player == null) return;

        Transform newTarget = FindNearestEnemy();

        if (newTarget != null && newTarget != currentTarget)
        {
            currentTarget = newTarget;
        }

        if (currentTarget != null)
        {
            Vector3 targetPos = new Vector3(currentTarget.position.x, player.position.y, currentTarget.position.z);
            player.LookAt(targetPos);
        }
    }

    Transform FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(player.position, detectionRange, enemyMask);
        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}
