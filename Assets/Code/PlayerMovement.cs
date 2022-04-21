using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask itemMask;
    [SerializeField] private float itemPickupRadius = 5f;
    [SerializeField, Range(0f, 1f)] private float strafeDot = 0.85f;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private bool canMove = true;
    private Inventory _inventory;
    private UIInventory uiInventory;
    private float verticalAcceleration = 0f;

    public Inventory inventory => _inventory;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        playerAnimator = new PlayerAnimator(transform, strafeDot);
        _inventory = new Inventory();
        uiInventory = GameObject.FindWithTag("PlayerInventory").GetComponent<UIInventory>();
        uiInventory.SetInventory(_inventory);
    }

    void Update()
    {
        if (canMove)
        {
            Move();
            Turn();
            Attack();

            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.OverlapSphere(transform.position, itemPickupRadius, itemMask.value).Length != 0)
            {
                if (Physics.Raycast(camRay, out hit, Mathf.Infinity, itemMask.value) && Input.GetKeyDown(KeyCode.E))
                {
                    var pickable = hit.collider.GetComponent<ItemPickable>();
                    inventory.Add(pickable.GetItem());
                    pickable.DestroySelf();
                }
            }
        }

        CalculateGravity();
    }

    private void Attack()
    {
        PointerEventData p = new PointerEventData(EventSystem.current);
        p.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();

        EventSystem.current.RaycastAll(p, list);

        if (list.Count == 0 && Input.GetButtonDown("Fire1"))
        {
            canMove = false;
            StartCoroutine(playerAnimator.Attacking(() => canMove = true));
        }
    }

    private void CalculateGravity()
    {
        if (characterController.isGrounded)
        {
            verticalAcceleration = 0f;
        }
        else
        {
            verticalAcceleration -= 9.81f * Time.deltaTime * Time.deltaTime;
            characterController.Move(new Vector3(0f, verticalAcceleration, 0f));
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

        Gizmos.DrawWireSphere(transform.position, itemPickupRadius);

        Gizmos.color = Color.red;

        float angle = Mathf.Rad2Deg * Mathf.Acos(strafeDot);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, angle, 0f) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, -angle, 0f) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 180f + angle, 0f) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 180f - angle, 0f) * transform.forward);
    }
}
