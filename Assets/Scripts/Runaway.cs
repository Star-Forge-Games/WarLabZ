using UnityEngine;
using UnityEngine.Animations;

public class Runaway : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Camera.main.GetComponent<PositionConstraint>().enabled = false; 
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.GetComponent<CharacterController>().Move(8 * Time.deltaTime * animator.transform.forward); 
    }


}
