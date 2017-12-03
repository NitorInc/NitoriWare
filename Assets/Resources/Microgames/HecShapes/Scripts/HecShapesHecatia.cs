using UnityEngine;

public class HecShapesHecatia : MonoBehaviour
{

    Animator animator;
    
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    public void SetStyle(HecShapesSlottable.Shape style)
    {
        this.animator.SetTrigger(style.ToString());
    }

    public void Win()
    {
        this.animator.SetTrigger("Win");
        MicrogameController.instance.setVictory(true, true);
    }

    public void Lose()
    {
        this.animator.SetTrigger("Lose");
        MicrogameController.instance.setVictory(false, true);
    }

}
