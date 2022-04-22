using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().canMove = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().canMove = true;
    }
}
