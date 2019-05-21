using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlash3DEnvironmentController : MonoBehaviour
{
    [SerializeField]
    private GameObject stairObject;
    [SerializeField]
    private Vector2 stairOffset;

    [SerializeField]
    private Transform cameraParent;
    [SerializeField]
    private float cameraDollySpeed;

    Vector3 StairOffset3D => new Vector3(0f, stairOffset.y, stairOffset.x);
    
	//void Start ()
 //   {
 //       for (int i = 0; i < stairParent.childCount; i++)
 //       {
 //           stairParent.GetChild(i).localPosition = StairOffset3D * i;
 //       }
	//}
	
	void Update ()
    {
        cameraParent.position += StairOffset3D.resize(cameraDollySpeed * Time.deltaTime);
        if (cameraParent.localPosition.y > stairOffset.y)
        {
            cameraParent.localPosition -= StairOffset3D;
        }
	}
}
