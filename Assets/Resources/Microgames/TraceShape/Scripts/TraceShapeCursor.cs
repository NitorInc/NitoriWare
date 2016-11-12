using UnityEngine;
using System.Collections;

public class TraceShapeCursor : MonoBehaviour
{

	private ParticleSystem traceParticles;

	void Start ()
	{
		traceParticles = GetComponent<ParticleSystem>();
	}
	
	void Update ()
	{
		updateTrace();
	}

	void updateTrace()
	{
		//Enable emmissions if mouse is held down
		ParticleSystem.EmissionModule emission = traceParticles.emission;
		emission.enabled = Input.GetMouseButton(0);

		//When mouse is first pressed, create a particle (otherwise one won't be created before the player moves their cursor)
		if (Input.GetMouseButtonDown(0))
			traceParticles.Emit(1);

		//Snap to cursor
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
	}

}
