using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth - damage > 0) currentHealth -= damage;
        else
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        GameObject.Destroy(gameObject);
    }
}
