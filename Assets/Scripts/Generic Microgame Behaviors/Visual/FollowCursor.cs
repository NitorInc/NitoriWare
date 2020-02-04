using UnityEngine;
using System.Collections;

public class FollowCursor : MonoBehaviour
{
    [SerializeField]
    private bool useLateUpdate = true;

	void Start()
	{
	
	}

    void Update()
    {
        if (!useLateUpdate)
            updatePosition();
    }

    void LateUpdate()
	{
        if (useLateUpdate)
            updatePosition();
	}

    void updatePosition()
    {
        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        cursorPosition.z = transform.position.z;
        transform.position = cursorPosition;
    }
}
