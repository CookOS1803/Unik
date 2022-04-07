using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkingSpeed = 5f;
    public float runningSpeed = 10f;
    public float jumpSpeed = 5f;
    public float mass = 20f;
    public float wallContactLength = 0.5f;
    private CharacterController characterController;
    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CalculateMovementFromInputs();
        CalculateGravity();
                
        if (characterController.enabled)
            characterController.Move(moveDirection * Time.deltaTime);
        else if (Physics.Linecast(transform.position, transform.position + transform.TransformDirection(Vector3.down) * wallContactLength, LayerMask.GetMask("Wall")))
        {
            characterController.enabled = true;
            rb.isKinematic = true;
        }
    }


    private void CalculateMovementFromInputs()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetButton("Sprint");
        float speed = isRunning ? runningSpeed : walkingSpeed;

        float curSpeedX = speed * Input.GetAxisRaw("Vertical");
        float curSpeedY = speed * Input.GetAxisRaw("Horizontal");

        moveDirection = forward * curSpeedX + new Vector3(0f, moveDirection.y, 0f) + right * curSpeedY;
        
        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            //moveDirection.y = jumpSpeed;
            characterController.enabled = false;
            rb.isKinematic = false;
            rb.velocity = (Vector3.up + moveDirection * 0.2f)  * 50f;
        }
    }
    
    private void CalculateGravity()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= mass * Time.deltaTime;
        }
        else if (moveDirection.y < 0f)
        {
            moveDirection.y = 0f;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.GetContact(0).normal)
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(Vector3.down) * wallContactLength);
    }
}
