using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private LayerMask floorMask;
    [SerializeField, Range(0f, 1f)] private float strafeDot = 0.85f;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        moveDirection = new Vector3(h, 0f, v);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        characterController.Move(moveDirection * speed * Time.deltaTime);
        
        AnimateMovement(v, h);
    }

    private void AnimateMovement(float v, float h)
    {
        float dot = Vector3.Dot(transform.forward, moveDirection);

        if (h == 0f && v == 0f)
        {
            animator.SetBool("IsMovingForward", false);
            animator.SetBool("IsMovingBackwards", false);
            animator.SetBool("IsMovingLeft", false);
            animator.SetBool("IsMovingRight", false);
        }
        else if (Mathf.Abs(dot) < strafeDot)
        {
            bool isLeft = Vector3.SignedAngle(transform.forward, moveDirection, transform.up) < 0f;

            animator.SetBool("IsMovingForward", false);
            animator.SetBool("IsMovingBackwards", false);
            animator.SetBool("IsMovingLeft", isLeft);
            animator.SetBool("IsMovingRight", !isLeft);
        }
        else
        {
            bool isForward = dot > 0f;

            animator.SetBool("IsMovingForward", isForward);
            animator.SetBool("IsMovingBackwards", !isForward);
            animator.SetBool("IsMovingLeft", false);
            animator.SetBool("IsMovingRight", false);
        }
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
        Gizmos.color = Color.red;

        float angle = Mathf.Rad2Deg * Mathf.Acos(strafeDot);

        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, angle, 0f) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, -angle, 0f) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 180f + angle, 0f) * transform.forward);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 180f - angle, 0f) * transform.forward);

        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position, moveDirection);
    }
}
