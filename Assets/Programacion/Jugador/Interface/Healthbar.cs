using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [Header("Healthbar Components")]
    [SerializeField] private GameObject healthbarFront;
    [SerializeField] private GameObject healthBarBack;
    [SerializeField] private GameObject healthBarChange;

    private Image HealthbarFrontImage;
    private Image HealthbarChangeImage;

    [Header("Visual Settings")]
    [SerializeField] private float reduceSpeed = 2f;
    [SerializeField] private Color AddColor;
    [SerializeField] private Color SubtractColor;

    private float targetFill;

    private void Start()
    {
        HealthbarFrontImage = healthbarFront.GetComponent<Image>();
        HealthbarChangeImage = healthBarChange.GetComponent<Image>();
        targetFill = HealthbarFrontImage.fillAmount;
    }

    public void UpdateHealthbar(float maxHealth, float currentHealth, bool isIncreasing)
    {
        targetFill = currentHealth / maxHealth;
        HealthbarFrontImage.fillAmount = targetFill;

        if (isIncreasing)
        {
            HealthbarChangeImage.color = AddColor;
        }
        else
        {
            HealthbarChangeImage.color = SubtractColor;
        }
    }

    void Update()
    {
        if (HealthbarChangeImage.fillAmount != targetFill)
        {
            HealthbarChangeImage.fillAmount = Mathf.MoveTowards(
                HealthbarChangeImage.fillAmount,
                targetFill,
                reduceSpeed * Time.deltaTime
            );
        }
    }
}
