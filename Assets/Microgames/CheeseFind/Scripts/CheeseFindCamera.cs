using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindCamera : MonoBehaviour {
    [Header("Camera offset")]
    [SerializeField]
    private float offset = 1f;

    private float _originY;
    private float _destinationY;
    private float _cameraFactor = 0f;
    private bool _isMoving = false;

    public void MoveCameraDown() {
        _isMoving = true;
    }

	void Start () {
        _destinationY = transform.position.y;
        _originY = _destinationY + offset;
        transform.position = new Vector3(transform.position.x, _originY, transform.position.z);
	}
	
	void Update () {
        if(!_isMoving)
            return;

        _cameraFactor +=  Time.deltaTime;
        if(_cameraFactor > 1f) {
            _cameraFactor = 1f;
            _isMoving = false;
        }

		transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(_originY, _destinationY, EaseInOutSine(_cameraFactor)),
            transform.position.z);
	}

    float EaseInOutSine(float t) {
        return (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
    }
}
