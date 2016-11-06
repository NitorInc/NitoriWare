using UnityEngine;
using System.Collections;

public class CameraTitle : MonoBehaviour
{

	public float offset;

	public Camera cam;

	void Awake ()
	{
		offset = cam.transform.localPosition.x;
	}
	
	void Update ()
	{
	
	}
}
