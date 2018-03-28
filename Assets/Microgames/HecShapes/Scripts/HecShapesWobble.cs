using UnityEngine;

public class HecShapesWobble : MonoBehaviour
{
    public float speed;
    public float magnitude;

    [SerializeField]
    bool wobble = true;
    
    float startTime;
    float offset;

    void Start()
    {
        Wobble();
    }

    void Update()
    {
        if (this.wobble)
        {
            float wobbleVector = Mathf.Sin(((Time.time - this.startTime) * this.speed) + Mathf.PI);
            float wobbleRotation = this.magnitude * wobbleVector;

            SetZRotation(wobbleRotation);
        }
    }

    void SetZRotation(float zRotation)
    {
        transform.localRotation = new Quaternion(
            transform.localRotation.x,
            transform.localRotation.y,
            zRotation,
            transform.localRotation.w
        );
    }

    public void Wobble()
    {
        this.startTime = Time.time;
        this.wobble = true;
    }

    public void Settle()
    {
        SetZRotation(0);
        this.wobble = false;
    }
}
