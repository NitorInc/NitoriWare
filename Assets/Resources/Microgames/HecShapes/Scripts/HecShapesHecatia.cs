using UnityEngine;

public class HecShapesHecatia : MonoBehaviour
{
    
    Animator animator;

    void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    public void SetStyle(HecShapesSlottable.Shape shape)
    {
        this.animator.SetInteger("Style", (int)shape);
        BroadcastMessage("SetShape", shape);
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
