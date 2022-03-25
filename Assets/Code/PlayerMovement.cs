using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkingSpeed = 5f;
    public float runningSpeed = 10f;
    public float jumpSpeed = 5f;
    public float mass = 20f;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetButton("Jump") && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= mass * Time.deltaTime;
        }
        else if (moveDirection.y < 0f)
        {
            moveDirection.y = 0f;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void CalculateMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetButton("Sprint");
        float speed = isRunning ? runningSpeed : walkingSpeed;

        float curSpeedX = speed * Input.GetAxisRaw("Vertical");
        float curSpeedY = speed * Input.GetAxisRaw("Horizontal");

        moveDirection = forward * curSpeedX + new Vector3(0f, moveDirection.y, 0f) + right * curSpeedY;
    }
}
