using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    private Animator animator;
    private Transform transform;
    
    public PlayerAnimator(Transform playerTransform)
    {
        transform = playerTransform;
        animator = transform.GetComponent<Animator>();
    }

    public void AnimateMovement(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
        moveDirection = Quaternion.Euler(0f, -angle, 0f) * moveDirection;
        
        animator.SetBool("isMoving", true);
        animator.SetFloat("forward", moveDirection.z, 0.1f, Time.deltaTime);
        animator.SetFloat("right", moveDirection.x, 0.1f, Time.deltaTime);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
