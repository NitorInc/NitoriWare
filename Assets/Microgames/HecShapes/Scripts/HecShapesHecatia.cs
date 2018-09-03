using UnityEngine;
using UnityEngine.Events;

public class HecShapesHecatia : MonoBehaviour
{

    [SerializeField]
    HecShapesHolder holder;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        holder.onFill = new UnityEvent();
    }

    public void SetStyle(HecShapesSlottable.Shape shape)
    {
        animator.SetInteger("Style", (int)shape);
        holder.SlotShape = shape;
    }

    public void MakeGray()
    {
        animator.SetTrigger("Gray");
    }

    public void AddOnFillAction(UnityAction fillAction)
    {
        holder.onFill.AddListener(fillAction);
    }

    public bool IsFilled()
    {
        return holder.Filled && holder.Valid;
    }

    public void Win()
    {
        animator.SetTrigger("Win");
    }

    public void Lose()
    {
        animator.SetTrigger("Lose");
    }

}
