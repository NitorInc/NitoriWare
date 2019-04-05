using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockCameraZoom : MonoBehaviour
{
    [SerializeField]
    private float goalCamSize;
    [SerializeField]
    private Vector3 goalPositionOffset;
    [SerializeField]
    private float duration;
    [SerializeField]
    private float startDelay;

    private float startCamSize;
    private Vector3 startPosition;
    private Vector3 goalPosition;
    private bool invoked;
    private bool started;
    private float progress;
    private Camera camera;
    
	void Start ()
    {
        camera = Camera.main;
	}

    void beginTransition()
    {
        started = true;
        progress = 0f;

        goalPosition = transform.position + goalPositionOffset;
        startPosition = camera.transform.position;
        startCamSize = camera.orthographicSize;
    }
	
	void Update ()
    {
		if (!invoked)
        {
            if (MicrogameController.instance.getVictory())
            {
                Invoke("beginTransition", startDelay);
                invoked = true;
            }
        }

        if (started)
        {
            progress += (1f / duration) * Time.deltaTime;
            if (progress >= 1f)
            {
                progress = 1f;
                enabled = false;
            }

            camera.transform.position = Vector3.Slerp(startPosition, goalPosition, progress);
            camera.orthographicSize = Mathf.Lerp(startCamSize, goalCamSize, progress);
        }
        

	}
}
