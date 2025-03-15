using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private bool isBlocked = false;  
    private Vector3 lastMoveDirection; 

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("No se ha encontrado un CharacterController en el GameObject.");
        }
    }

    void Update()
    {
        if (controller == null)
        {
            Debug.LogError("El CharacterController no está asignado correctamente.");
            return;
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -1f;
        }

        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        lastMoveDirection = move;

       
        if (!isBlocked)
        {
            
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))  
            {
                if (hit.collider.CompareTag("Wall"))  
                {
                    isBlocked = true;  
                }
            }

           
            if (move != Vector3.zero && !isBlocked)
            {
                controller.Move(move * Time.deltaTime * playerSpeed);
                transform.forward = move.normalized;  
            }
        }
        else
        {
            
            Vector3 newMove = new Vector3(0, 0, 0);

            
            newMove.x = Input.GetAxis("Horizontal"); 

            
            if (move.z > 0)
            {
                newMove.z = 0;  
            }
            else
            {
                newMove.z = Input.GetAxis("Vertical");  
            }

            if (newMove != Vector3.zero)
            {
                controller.Move(newMove * Time.deltaTime * playerSpeed);
                transform.forward = newMove.normalized;  
            }

            
            if (Input.GetKeyDown(KeyCode.B))
            {
                isBlocked = false;  
            }
        }

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);
    }


   
    public void BlockPlayer(bool block)
    {
        isBlocked = block;
    }
}
