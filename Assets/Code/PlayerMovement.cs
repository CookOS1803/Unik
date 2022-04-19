using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask floorMask;
    private CharacterController characterController;
    private PlayerAnimator playerAnimator;
    private Vector3 moveDirection;
    private bool canMove = true;
    private Inventory inventory;
    private UIInventory uiInventory;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();

        inventory = new Inventory();
        uiInventory = GameObject.FindWithTag("PlayerInventory").GetComponent<UIInventory>();
        uiInventory.SetInventory(inventory);
    }

    void Update()
    {
        if (canMove)
        {
            Move();
            Turn();

            if (Input.GetButtonDown("Fire1"))
            {
               canMove = false;
               playerAnimator.PlayAttack(() => canMove = true);
            }
        }                
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        moveDirection = new Vector3(h, 0f, v);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        characterController.Move(moveDirection * speed * Time.deltaTime);
        
        playerAnimator.AnimateMovement(moveDirection);
    }  

    private void Turn()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, Mathf.Infinity, floorMask.value))
        {
            Vector3 hitPoint = floorHit.point;
            hitPoint.y = transform.position.y;
            transform.LookAt(hitPoint);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, moveDirection);
    }
}
