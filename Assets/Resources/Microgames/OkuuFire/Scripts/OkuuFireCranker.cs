using UnityEngine;

public class OkuuFireCranker : MonoBehaviour
{
    private Animator animator;

    private float startAngle;

    void Start ()
    {
        this.animator = this.GetComponent<Animator>();
        this.animator.SetFloat("Speed", 0);

        this.Rotate(this.startAngle);
	}

    public void SetStartAngle(float angle)
    {
        if (this.animator != null)
            this.Rotate(angle);
        else
            this.startAngle = angle;
    }
	
    public void Rotate(float angle)
    {
        angle = (angle + 232) % 360;
        float crankedness = 1 - (angle / 360);
        this.animator.SetFloat("Cycle Offset", crankedness);
    }

}
