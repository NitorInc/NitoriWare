using UnityEngine;
using UnityEngine.Events;

public class HecShapesHecatia : MonoBehaviour
{

    [SerializeField]
    HecShapesHolder holder;

    Animator animator;

    void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.holder.onFill = new UnityEvent();
    }

    public void SetStyle(HecShapesSlottable.Shape shape)
    {
        this.animator.SetInteger("Style", (int)shape);
        this.holder.SlotShape = shape;
    }

    public void AddOnFillAction(UnityAction fillAction)
    {
        this.holder.onFill.AddListener(fillAction);
    }

    public bool IsFilled()
    {
        return this.holder.Filled && this.holder.Valid;
    }

    public void Win()
    {
        this.animator.SetTrigger("Win");
    }

    public void Lose()
    {
        this.animator.SetTrigger("Lose");
    }

}
