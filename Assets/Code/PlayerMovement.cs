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
    public Vector3 wallContactOrigin;
    public LayerMask wallContactLayer;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    public Vector3 RelativeContactOrigin => transform.TransformPoint(wallContactOrigin);

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Linecast(RelativeContactOrigin, RelativeContactOrigin + transform.TransformDirection(Vector3.left) * wallContactLength, out hit, wallContactLayer.value))
        {
            Vector3 t = hit.point - RelativeContactOrigin;
            //Euler
            moveDirection = new Vector3(t.z, 0f, -t.x) * walkingSpeed + t;
            
        }
        else
        {
            CalculateMovementFromInputs();
            CalculateGravity();
        }
        
        characterController.Move(moveDirection * Time.deltaTime);
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
            moveDirection.y = jumpSpeed;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 pos = RelativeContactOrigin;
        Gizmos.DrawLine(pos, pos + transform.TransformDirection(Vector3.left) * wallContactLength);
        Gizmos.DrawLine(pos, pos + transform.TransformDirection(Vector3.right) * wallContactLength);
    }
}
