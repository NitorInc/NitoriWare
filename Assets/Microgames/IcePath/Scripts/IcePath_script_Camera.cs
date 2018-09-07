using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_script_Camera : MonoBehaviour {
    float pi = Mathf.PI;

    [Header("Camera")]
    [SerializeField] private GameObject camera;
    Camera cameraComponent;
    private float cameraSize;

    [Header("Gentle sway customize")]
    [SerializeField] private float speed;
    [SerializeField] private float magnitude;
    private float counter;

    bool zoomOut = false;
    
    void Start () {
        // Get the component
        cameraComponent = camera.GetComponent<Camera>();

        // Camera stuff
        cameraSize = 0.75f;

        Invoke("ZoomOut", 60 * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
        // Size
        cameraComponent.orthographicSize = cameraSize;

        if (zoomOut) {
            // Position
            transform.position = Vector2.Lerp(transform.position, new Vector2(0, 0), 0.1f);
            cameraSize = Mathf.Lerp(cameraSize, 5f, 0.1f);
        }
    }

    void ZoomOut() {
        // Zoom
        zoomOut = true;
    }
}
