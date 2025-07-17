using UnityEngine;

public class PlayerAnim : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Weapon.instance.SelfPause();  
    }



    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.Rotate(0, 180, 0);
    }


}
