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
        if (wobble)
        {
            float wobbleVector = Mathf.Sin(((Time.time - startTime) * speed) + Mathf.PI);
            float wobbleRotation = magnitude * wobbleVector;

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
        startTime = Time.time;
        wobble = true;
    }

    public void Settle()
    {
        SetZRotation(0);
        wobble = false;
    }
}
