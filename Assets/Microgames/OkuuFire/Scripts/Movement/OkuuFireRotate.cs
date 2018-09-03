using UnityEngine;

public class OkuuFireRotate : MonoBehaviour
{

    public float rotateSpeed;
    public float maxRotation;
    public float offset;
    
    private float startTime;

    [SerializeField]
    private bool rotate;

    void Start()
    {
        Reset();
    }
    
    void Update()
    {
        if (rotate)
        {
            DoRotate();
        }
    }

    void Reset()
    {
        startTime = Time.time;
        DoRotate();
    }

    void DoRotate()
    {
        float rotateAmount = Mathf.Sin(((Time.time - startTime) * rotateSpeed) + (offset * Mathf.PI));
        
        float angle = maxRotation * rotateAmount;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
