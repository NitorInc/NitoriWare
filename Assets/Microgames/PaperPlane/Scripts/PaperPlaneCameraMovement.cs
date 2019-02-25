using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneCameraMovement : MonoBehaviour {
    [SerializeField]
    Vector4 ViewportBorder =  new Vector4 (-11.4f, -7, 11.4f, 7);
    [SerializeField]
    Transform target;
    Vector2 cameraSize
    {
        get
        {
            return new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
        }
    }
    
	void Update () {
        
            Movestep();
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
