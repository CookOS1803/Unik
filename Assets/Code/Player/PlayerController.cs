using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask itemMask;
    [SerializeField] private float itemPickupRadius = 5f;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;
    private PlayerWeapon weapon;
    private Vector3 moveDirection;
    [Inject] private UIInventory uiInventory;
    private float verticalAcceleration = 0f;
    private KeyCode[] numberCodes;
    public bool canMove { get; set; } = true;

    [Inject] public Inventory inventory { get; private set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = new PlayerAnimator(transform);
        weapon = GetComponentInChildren<PlayerWeapon>();
        inventory.owner = transform;
        uiInventory.SetInventory(inventory);

        numberCodes = new KeyCode[]
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
            KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
            KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
            KeyCode.Alpha0
        };
    }

    void Update()
    {
        if (canMove)
        {
            Move();
            Turn();
            Attack();
            PickItem();
            UseItem();
        }

        CalculateGravity();
        SelectItemSlot();
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

    private void Attack()
    {
        if (!UserRaycaster.IsBlockedByUI() && Input.GetButtonDown("Fire1"))
        {
            playerAnimator.Attack();
        }
    }    
    
    private void PickItem()
    {
        if (inventory.IsFull())
            return;

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.OverlapSphere(transform.position, itemPickupRadius, itemMask.value).Length != 0)
        {
            if (Physics.Raycast(camRay, out hit, Mathf.Infinity, itemMask.value) && Input.GetButtonDown("Action"))
            {
                var pickable = hit.collider.GetComponent<ItemPickable>();
                inventory.Add(pickable.GetItem());
                pickable.DestroySelf();
            }
        }
    }

    private void UseItem()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            inventory.UseItem();
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

    private void SelectItemSlot()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel == 0)
        {
            for (int i = 0; i < numberCodes.Length; i++)
            {
                if (Input.GetKeyDown(numberCodes[i]))
                {
                    inventory.selectedSlot = i;
                    return;
                }
            }

            return;
        }

        if (wheel > 0)
        {
            inventory.selectedSlot = (inventory.selectedSlot + 1) % inventory.size;
        }
        else
        {
            inventory.selectedSlot = (inventory.size + inventory.selectedSlot - 1) % inventory.size;
        }

    }

    public void OnAttackStartEvent()
    {
        weapon.StartDamaging();
    }

    public void OnAttackEndEvent()
    {
        weapon.StopDamaging();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, moveDirection);

        Gizmos.DrawWireSphere(transform.position, itemPickupRadius);
    }
}
