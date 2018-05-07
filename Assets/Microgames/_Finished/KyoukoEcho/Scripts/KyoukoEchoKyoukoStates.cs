using UnityEngine;

public class KyoukoEchoKyoukoStates : StateMachineBehaviour
{

    [SerializeField]
    bool wound;
    [SerializeField]
    AudioClip hitClip;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Play hit sound
        MicrogameController.instance.playSFX(hitClip);

        if (wound && !animator.GetBool("Wounded"))
        {
            animator.SetBool("Wounded", true);
        }
    }

}
