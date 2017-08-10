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
        this.Reset();
    }
    
    void Update()
    {
        if (this.rotate)
        {
            this.DoRotate();
        }
    }

    void Reset()
    {
        this.startTime = Time.time;
        this.DoRotate();
    }

    void DoRotate()
    {
        float rotateAmount = Mathf.Sin(((Time.time - this.startTime) * this.rotateSpeed) + (this.offset * Mathf.PI));
        
        float angle = this.maxRotation * rotateAmount;
        this.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

}
