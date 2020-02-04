using UnityEngine;
using System.Collections;

public class VictoryResultAnimation : MonoBehaviour
{
    // Sets a trigger or bool microgame on victory or loss
    // Won't activate an empty string trigger/bool

    [SerializeField]
    private string victoryTrigger = "Victory";
    [SerializeField]
    private string lossTrigger = "Loss";
    [SerializeField]
    private string victoryStatusBool;
    [SerializeField]
    private string victoryDeterminedTrigger;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            if (!string.IsNullOrEmpty(victoryDeterminedTrigger))
                animator.SetTrigger(victoryDeterminedTrigger);
            string resultTrigger = MicrogameController.instance.getVictory() ? victoryTrigger : lossTrigger;
            if (!string.IsNullOrEmpty(resultTrigger))
                animator.SetTrigger(resultTrigger);
            if (!string.IsNullOrEmpty(victoryStatusBool))
                animator.SetBool(victoryStatusBool, MicrogameController.instance.getVictory());
            enabled = false;
        }
        else
        {
            if (!string.IsNullOrEmpty(victoryStatusBool))
                animator.SetBool(victoryStatusBool, MicrogameController.instance.getVictory());
        }
    }
}
