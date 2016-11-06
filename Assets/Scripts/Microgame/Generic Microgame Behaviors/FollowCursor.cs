using UnityEngine;
using System.Collections;

public class FollowCursor : MonoBehaviour
{

	void Start()
	{
	
	}
	
	void LateUpdate()
	{
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		cursorPosition.z = transform.position.z;
		transform.position = cursorPosition;
	}
}
