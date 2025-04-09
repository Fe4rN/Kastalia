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
                Debug.Log("[CHARGE] Botón derecho presionado - Iniciando carga");
                espadachin.isRightMouseDown = true;
                StartCoroutine(espadachin.ChargeSword());
            }

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Debug.Log($"[CHARGE] Botón derecho liberado - Tiempo cargado: {espadachin.chargeTime.ToString("F2")}s");
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
                    float damage = playerInventory.weapon.damage;
                    Debug.Log($"[ATTACK] Daño base: {damage}");


                    if (espadachin.isFullyCharged)
                    {
                        damage *= espadachin.chargeMultiplier;
                        Debug.Log($"[ATTACK] Ataque CARGADO aplicado! Daño final: {damage}");

                    }
                    if (espadachin.isChargingSword)
                    {
                        Debug.Log($"[ATTACK] Ataque INTERRUMPIDO (carga no completada)");


                        //daño suave
                        damage *= 0.5f;

                    }
                    else
                    {
                        Debug.Log("[ATTACK] Ataque NORMAL");
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
