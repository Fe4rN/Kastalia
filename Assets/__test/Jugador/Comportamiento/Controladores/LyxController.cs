using UnityEngine;

public class LyxController : PlayerController
{
    private Espadachin espadachin;

    protected override void Start()
    {
        base.Start();
        espadachin = GetComponent<Espadachin>();
    }
    protected override void Update()
    {
        base.Update();
        if(playerInventory.weapon && playerInventory.selectedItemType == ItemType.Arma)
        {
            // Manejo del click derecho para cargar el ataque de espada
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                espadachin.isRightMouseDown = true;
                StartCoroutine(espadachin.ChargeSword());
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                espadachin.isRightMouseDown = false;
                if (espadachin.isChargingSword)
                {
                    StopCoroutine(espadachin.ChargeSword());
                    espadachin.isChargingSword = false;
                    espadachin.chargeTime = 0f;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!isAttacking && !isDashing)
                {
                    int damage = playerInventory.weapon.damage;


                    if (espadachin.isFullyCharged)
                    {
                        damage *= espadachin.chargeMultiplier;

                    }
                    if (espadachin.isChargingSword)
                    {


                        //daño suave
                        damage = Mathf.CeilToInt(damage * 0.5f);

                    }

                    // damage *= 1.5f;

                    StartCoroutine(espadachin.SwordAttack(damage));

                    // Resetear estados de carga
                    espadachin.isChargingSword = false;
                    espadachin.isFullyCharged = false;
                    espadachin.chargeTime = 0f;
                }
            }
        }
    }
}
