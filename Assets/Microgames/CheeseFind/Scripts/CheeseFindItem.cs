using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindItem : MonoBehaviour {
    //TODO: Select a random sprite from a list

    private bool _isUsed = false;
    public bool isUsed {
        get { return _isUsed; }
        set { _isUsed = value; }
    }

    public Vector3 drawerPosition;

    private bool _isMoving = false;
    private float _movingTime = 0f;
    private float _movingDuration = 0f;
    private Vector3 _origin;
    private Vector3 _destination;

    private bool _isHovering = true;
    private Vector3 _position;
    
	void Start () {
		_position = transform.position;
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
        else if(_isHovering) {
            transform.position = _position + new Vector3(0f, Mathf.Sin(Time.time * 15f) * .1f, 0f);
        }
	}

    public void MoveTo(Vector3 destination, float duration) {
        _isHovering = false;
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

    public void MoveAway(float duration) {
        MoveTo(transform.position + new Vector3(0f, 10f, 0f), duration);
    }

    float EaseInOutSine(float t) {
        return (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
    }
}
