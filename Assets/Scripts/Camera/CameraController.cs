using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public static CameraController instance;

	private Camera _camera;

	void Awake ()
	{
		_camera = GetComponent<Camera>();
		_camera.aspect = 4f / 3f;
	}
}
