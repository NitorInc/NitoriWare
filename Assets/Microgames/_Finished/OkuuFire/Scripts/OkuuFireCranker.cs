using UnityEngine;

public class OkuuFireCranker : MonoBehaviour
{
    private Animator animator;

    private float startAngle;

    void Start ()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", 0);

        Rotate(startAngle);
	}

    public void SetStartAngle(float angle)
    {
        if (animator != null)
            Rotate(angle);
        else
            startAngle = angle;
    }
	
    public void Rotate(float angle)
    {
        angle = (angle + 232) % 360;
        float crankedness = 1 - (angle / 360);
        animator.SetFloat("Cycle Offset", crankedness);
    }

}
