using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IMoveable, IMortal
{
    public event Action onHide;
    public event Action onExitHideout;
    public event Action onDeath;

    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private LayerMask itemMask;
    [SerializeField] private LayerMask hideoutMask;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private float itemPickupRadius = 5f;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;
    private Health health;
    private Weapon weapon;
    private Vector3 moveDirection;
    private float verticalAcceleration = 0f;
    private KeyCode[] numberCodes;
    private bool isDying = false;
    public Transform currentHideout { get; private set; }
    public bool canMove { get; set; } = true;

    [Inject] public Inventory inventory { get; private set; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        playerAnimator = new PlayerAnimator(transform);
        weapon = GetComponentInChildren<Weapon>();

        health.onDeath += Die;

        inventory.owner = transform;

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
        if (currentHideout != null)
        {
            if (Input.GetButtonDown("Action"))
                ExitHideout();
        }
        else
        {
            CalculateGravity();
            
            if (canMove && !isDying)
            {
                Move();
                Turn();
                Attack();
                PickItem();
                UseItem();
                Hide();
                Interact();
            }
        }

        SelectItemSlot();
    }

    public void ExitHideout()
    {
        transform.position += currentHideout.forward;
        currentHideout = null;
        characterController.enabled = true;

        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(true);
        }

        onExitHideout?.Invoke();
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
        if (Input.GetButtonDown("Fire1") && !UserRaycaster.IsBlockedByUI())
        {
            playerAnimator.Attack();
        }
    }    
    
    private void PickItem()
    {
        if (inventory.IsFull())
            return;

        if (Physics.OverlapSphere(transform.position, itemPickupRadius, itemMask.value).Length != 0)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetButtonDown("Action") && Physics.Raycast(camRay, out hit, Mathf.Infinity, itemMask.value))
            {
                if (!Physics.Linecast(transform.position + Vector3.up, hit.point, ~itemMask))
                {
                    var pickable = hit.collider.GetComponent<ItemPickable>();
                    inventory.Add(pickable.GetItem());
                    pickable.DestroySelf();
                }
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
    
    private void Hide()
    {
        if (Physics.OverlapSphere(transform.position, itemPickupRadius, hideoutMask.value).Length != 0)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetButtonDown("Action") && Physics.Raycast(camRay, out hit, Mathf.Infinity, hideoutMask.value))
            {
                if (!Physics.Linecast(transform.position + Vector3.up, hit.point, ~hideoutMask))
                {
                    currentHideout = hit.transform;
                    characterController.enabled = false;

                    foreach (Transform c in transform)
                    {
                        c.gameObject.SetActive(false);
                    }

                    transform.position = currentHideout.position;

                    onHide?.Invoke();
                }
            }
        }
    }

    private void Interact()
    {
        if (Physics.OverlapSphere(transform.position, itemPickupRadius, interactableMask.value).Length != 0)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetButtonDown("Action") && Physics.Raycast(camRay, out hit, Mathf.Infinity, interactableMask.value))
            {
                if (!Physics.Linecast(transform.position + Vector3.up, hit.point, ~interactableMask))
                {
                    hit.transform.GetComponent<IInteractable>().Use(transform);
                }
            }
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

    void Die()
    {
        gameObject.layer = 0;
        isDying = true;
        canMove = false;
        playerAnimator.Die();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, moveDirection);

        Gizmos.DrawWireSphere(transform.position, itemPickupRadius);
    }

    public void OnDeath()
    {   
        onDeath?.Invoke();

        Destroy(this);
    }
}
