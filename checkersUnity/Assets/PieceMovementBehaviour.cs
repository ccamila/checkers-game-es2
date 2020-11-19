using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementBehaviour : StateMachineBehaviour
{


    public float movementLengthX;
    public float movementLengthZ;
    public float transitionSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        Debug.Log("Iniciando Behaviour");
        GameObject gobj = animator.gameObject;
        //movementLengthX = animator.GetInt("MovementX");
        //movementLengthY = animator.GetInt("MovementY");

        //movementLengthZ = 2f;
        //movementLengthX = 2f;

        //Debug.Log("Length X = " + movementLengthX);
        //Debug.Log("Length Z = " + movementLengthZ);

        Vector3 newPos = new Vector3(animator.gameObject.transform.position.x + movementLengthX, animator.gameObject.transform.position.y,
                                    animator.gameObject.transform.position.z + movementLengthZ);


        animator.gameObject.transform.parent.gameObject.GetComponent<DummyMovement>().SetDestination(newPos, transitionSpeed);


        animator.SetBool("IsMoving", false);*/
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    /*override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.Translate(Vector3.forward * Time.deltaTime);
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Debug.Log("Terminou Behaviour");
        

    }
    */
    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
