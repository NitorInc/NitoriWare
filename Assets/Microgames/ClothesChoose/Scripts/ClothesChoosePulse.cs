using UnityEngine;

public class ClothesChoosePulse : MonoBehaviour
{
    public float pulseSpeed;

    public float minX = 1;
    public float maxX = 1;
    public float minY = 1;
    public float maxY = 1;

    [SerializeField]
    private bool pulse = true;

    private float startTime;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (pulse)
        {
            float pulseAmount = Mathf.Sin(((Time.time - startTime) * pulseSpeed) + Mathf.PI);
            pulseAmount = (pulseAmount + 1) / 2;

            float deltaX = (maxX - minX) * pulseAmount;
            float deltaY = (maxY - minY) * pulseAmount;

            float newXScale = minX + deltaX;
            float newYScale = minY + deltaY;

            transform.localScale = new Vector3(newXScale, newYScale);
        }
    }

    void Reset()
    {
        startTime = Time.time;
    }
}
