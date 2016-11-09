using UnityEngine;
using System.Collections;

public class SineWave : MonoBehaviour
{

	//Moves the object in a sine wave relative to parent, allows both x and y wave
	//Attach to a parent object, because it will directly edit localPosition

	public float xSpeed, xAmplitude, xOffset, ySpeed, yAmplitude, yOffset;

	
	void Update ()
	{
		float x = transform.position.x, y = transform.position.y;
		if (xAmplitude > 0f)
		{
			x = Mathf.Sin((Time.time * xSpeed) + (xOffset * Mathf.PI)) * xAmplitude;
		}
		if (yAmplitude > 0f)
		{
			y = Mathf.Sin((Time.time * ySpeed) + (yOffset * Mathf.PI)) * yAmplitude;
		}
		transform.localPosition = new Vector3(x, y, transform.localPosition.z);
	}
}