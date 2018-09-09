using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_script_Camera : MonoBehaviour {
    private float pi = Mathf.PI;

    [Header("Camera")]
    [SerializeField] private GameObject camera;
    [SerializeField] private int zoomOutTime;

    private Camera   cameraComponent;
    private float    cameraSize;

    [HideInInspector] public int zoomState = 0;
    
    void Start () {
        // Get the component
        cameraComponent = camera.GetComponent<Camera>();

        // Camera stuff
        cameraSize = 0.75f;

        Invoke("ZoomOut", zoomOutTime * Time.deltaTime);
	}
	
	void Update () {
        // Size
        cameraComponent.orthographicSize = cameraSize;

        // Zoom state
        // Zoom out from ice cream
        if (zoomState == 1) {
            cameraSize = Mathf.Lerp(cameraSize, 5f, 0.1f);
            transform.position = Vector2.Lerp(transform.position, new Vector2(0, 0), 0.1f);

        } else
        
        // Zoom in to Cirno for victory
        if (zoomState == 2) {
            GameObject cirno = GameObject.Find("IcePath_prefab_Cirno(Clone)");

            cameraSize = Mathf.Lerp(cameraSize, 1.25f, 0.1f);
            transform.position = Vector2.Lerp(transform.position, cirno.transform.position + new Vector3(0, 0.25f, 0), 0.1f);

        }
    }

    void ZoomOut() {
        // Zoom out
        zoomState = 1;
    }
}
