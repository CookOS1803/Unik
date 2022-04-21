using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    private Animator animator;
    private Transform transform;
    private float strafeDot;
    
    public PlayerAnimator(Transform playerTransform, float newStrafeDot)
    {
        transform = playerTransform;
        animator = transform.GetComponent<Animator>();
        strafeDot = newStrafeDot;
    }

    public void AnimateMovement(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero)
        {
            animator.SetBool("IsMovingForward", false);
            animator.SetBool("IsMovingBackwards", false);
            animator.SetBool("IsMovingLeft", false);
            animator.SetBool("IsMovingRight", false);

            return;
        }

        float dot = Vector3.Dot(transform.forward, moveDirection);
        
        if (Mathf.Abs(dot) < strafeDot)
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

    public IEnumerator Attacking(System.Action action)
    {
        animator.SetTrigger("Attack");
    
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        yield return new WaitWhile(() => animator.IsInTransition(0));

        action();
    }
}
