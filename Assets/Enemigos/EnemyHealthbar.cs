using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthbar : MonoBehaviour
{
    [SerializeField] private Image healthbar;
    [SerializeField] private float reduceSpeed = 2f;
    private float target = 1;

    private Camera cam;
    private void Start() {
        cam = Camera.main;
    }

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
    }

    void Update()
    {
        if (GameManager.instance.isPaused) return;
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthbar.fillAmount = Mathf.MoveTowards(healthbar.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
