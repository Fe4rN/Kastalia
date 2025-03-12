using System;
using System.Collections;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float dashCooldown = 2;
    private bool isDashing = false;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed = 1.5f;
    [SerializeField] private float dashTime = 0.25f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintValue : 1f;
        controller.Move(move * Time.deltaTime * playerSpeed * speedMultiplier);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing) StartCoroutine(Dash(move));
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator Dash(Vector3 move)
    {

        float startTime = Time.time;
        isDashing = true;
        while (Time.time < startTime + dashTime)
        {
            Debug.Log("Dash");
            controller.Move(move * dashSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(dashCooldown - dashTime);
        isDashing = false;
    }
}
