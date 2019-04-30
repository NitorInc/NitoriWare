using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainCameraSingleton : MonoBehaviour
{
    private const string MainCameraTag = "MainCamera";

    private static List<Camera> cameraInstances;

    private Camera camera;
    
    public static Camera instance
    {
        get
        {
            if (cameraInstances == null)
                cameraInstances = new List<Camera>();

            var markedForRemoval = new List<Camera>();
            Camera returnCamera = null;
            foreach (var camera in cameraInstances)
            {
                if (camera == null)
                    markedForRemoval.Add(camera);
                else if (camera.tag == "MainCamera")
                {
                    returnCamera = camera;
                    break;
                }
            }
            if (markedForRemoval.Any())
                cameraInstances = cameraInstances.Except(markedForRemoval).ToList();
            return returnCamera;
        }
    }

	void Awake()
    {
        cameraInstances = new List<Camera>();
        camera = GetComponent<Camera>();
        cameraInstances.Add(camera);
	}
    private void OnDestroy()
    {
        cameraInstances.Remove(camera);
    }
}
