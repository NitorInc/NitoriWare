using UnityEngine;

public class KyoukoEchoKyoukoStates : StateMachineBehaviour
{

    [SerializeField]
    bool wound;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (this.wound && !animator.GetBool("Wounded"))
        {
            animator.SetBool("Wounded", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("Pose"))
        {
            animator.SetBool("Pose", true);
        }
    }

}
