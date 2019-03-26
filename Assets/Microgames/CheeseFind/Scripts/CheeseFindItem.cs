using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindItem : MonoBehaviour {

    //TODO: Select a random sprite from a list
    //TODO: Item Movement

    private bool _isMoving = false;
    private float _movingTime = 0f;
    private float _movingDuration = 0f;
    private Vector3 _origin;
    private Vector3 _destination;
    
	void Start () {
		
	}
	
	void Update () {
        if(_isMoving) {
            _movingTime += Time.deltaTime;
            float movingFactor = _movingTime / _movingDuration;
            if(_movingTime > _movingDuration) {
                movingFactor = 1f;
                _isMoving = false;
            }
		    transform.position = Vector3.Lerp(_origin, _destination, EaseInOutSine(movingFactor));
        }
	}

    public void MoveTo(Vector3 destination, float duration) {
        if(duration <= 0f) {
            transform.position = destination;
            return;
        }
        _isMoving = true;
        _movingTime = 0f;
        _movingDuration = duration;
        _origin = transform.position;
        _destination = destination;
    }

    float EaseInOutSine(float t) {
        return (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
    }
}
