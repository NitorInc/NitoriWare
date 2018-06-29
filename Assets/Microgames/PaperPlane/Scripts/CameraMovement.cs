using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    Vector4 ViewportBorder =  new Vector4 (-11.4f, -7, 11.4f, 7);
    [SerializeField]
    Transform target;
    [SerializeField]
    Vector2 SizeMorph = new Vector2(7, 5);
    [SerializeField]
    float delayBeforeZoom;
    [SerializeField]
    float ZoomTime;
    float timeElapsed;
    bool IsZooming;

    Vector2 cameraSize
    {
        get
        {
            return new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        }
    }

    void Start ()
    {
        StartCoroutine(Zoom());
    }
	
	// Update is called once per frame
	void Update () {

        if (IsZooming == false)
        {
            Movestep();
        }
	}

    IEnumerator Zoom ()
    {
        IsZooming = true;
        Movestep();
        yield return new WaitForSeconds(delayBeforeZoom);
        timeElapsed = 0;
        while(ZoomTime > timeElapsed)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
            Camera.main.orthographicSize = Mathf.Lerp(SizeMorph.x, SizeMorph.y, timeElapsed / ZoomTime);
            Movestep();
        }
        IsZooming = false;
    }

    private void Movestep()
    {
        Vector2 step = new Vector2(target.position.x, target.position.y);
        if (step.x - cameraSize.x < ViewportBorder.x)
            step.x = (ViewportBorder.x + cameraSize.x);
        if (step.x + cameraSize.x > ViewportBorder.z)
            step.x = (ViewportBorder.z - cameraSize.x);
        if (step.y - cameraSize.y < ViewportBorder.y)
            step.y = (ViewportBorder.y + cameraSize.y);
        if (step.y + cameraSize.y > ViewportBorder.w)
            step.y = (ViewportBorder.w - cameraSize.y);

        transform.position = new Vector3(step.x, step.y, transform.position.z);
    }
}
