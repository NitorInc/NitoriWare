using UnityEngine;
using System.Collections;

public class SineWave : MonoBehaviour
{

	//Moves the object in a sine wave relative to parent, allows both x and y wave
	//Attach to a parent object, because it will directly edit localPosition

	public float xSpeed, xAmplitude, xOffset, ySpeed, yAmplitude, yOffset;
	public bool relativeToStartPosition = true;

    [SerializeField]
	private Vector3 positionOffset;
	private float startTime;
    [SerializeField]
    private float xPow = 1f;
    [SerializeField]
    private float yPow = 1f;

    void Awake()
	{
		resetStartPosition();
		resetCycle();
	}

	public void resetCycle()
	{
		startTime = Time.time;
		Update();
	}

	public void resetStartPosition()
	{
        if (relativeToStartPosition)
		    positionOffset = transform.localPosition;
	}

	public void setStartPosition(Vector3 position)
	{
		positionOffset = position;
	}
	
	void Update ()
	{
		float x = transform.localPosition.x - positionOffset.x, y = transform.localPosition.y - positionOffset.y;
		if (xAmplitude > 0f)
		{
			x = Mathf.Sin(((Time.time - startTime) * xSpeed) + (xOffset * Mathf.PI)) * xAmplitude;
            if (xPow != 1f)
                x = Mathf.Pow(Mathf.Abs(x), xPow) * Mathf.Sign(x);
		}
		if (yAmplitude > 0f)
		{
			y = Mathf.Sin(((Time.time - startTime) * ySpeed) + (yOffset * Mathf.PI)) * yAmplitude;

            if (yPow != 1f)
                y = Mathf.Pow(Mathf.Abs(y), yPow) * Mathf.Sign(y);
        }
		transform.localPosition = new Vector3(x, y, 0f) + positionOffset;
	}

    /// <summary>
    /// Change speed. Only works when X and Y speed remain the same proportions
    /// </summary>
    /// <param name="speed"></param>
    public void setSpeed(Vector2 newSpeed)
    {
        float mult;
        if (xSpeed > 0f)
            mult = newSpeed.x / xSpeed;
        else if (ySpeed > 0f)
            mult = newSpeed.y / ySpeed;
        else
            return;
        float timeSinceStart = Time.time - startTime;
        timeSinceStart /= mult;
        startTime = Time.time - timeSinceStart;
        xSpeed = newSpeed.x;
        ySpeed = newSpeed.y;
    }
}