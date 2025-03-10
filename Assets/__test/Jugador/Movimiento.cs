using System;
using System.Collections;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float dashCooldown;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintValue = 2.0f;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
 
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift)? sprintValue : 1f;
        controller.Move(move * Time.deltaTime * playerSpeed * speedMultiplier);
        if(Input.GetKey(KeyCode.Space)){
            StartCoroutine(Dash(move));
        }
        
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator Dash(Vector3 move){
        float startTime = Time.time;
        while(Time.time < startTime + dashTime){
            controller.Move(move * dashSpeed *Time.deltaTime);
            yield return null;
        }
    }
}
