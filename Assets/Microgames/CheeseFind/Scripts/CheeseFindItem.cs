using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindItem : MonoBehaviour {
    private bool _isUsed = false;
    public bool isUsed {
        get { return _isUsed; }
        set { _isUsed = value; }
    }

    private bool _isCheese = false;
    public bool isCheese {
        get { return _isCheese; }
        set { _isCheese = value; 
            _cheeseRig.SetActive(_isCheese);
            _mouseRig.SetActive(!_isCheese);
        }
    }

    private float _angleFactor = 0f;
    public float angleFactor {
        get { return _angleFactor; }
        set { _angleFactor = value; }
    }

    public Vector3 drawerPosition;

    private bool _isMoving = false;
    private float _movingTime = 0f;
    private float _movingDuration = 0f;
    private Vector3 _origin;
    private Vector3 _destination;

    private bool _isHovering = true;
    private Vector3 _position;

    private bool _hasAnimation = false;
    private float _rotationDirection;
    private Vector3 _nominalScale;

    private GameObject _cheeseRig;
    private GameObject _mouseRig;

    void Awake() {
        _cheeseRig = transform.Find("Cheese").gameObject;
        _mouseRig = transform.Find("Mouse").gameObject;
    }
    
	void Start () {
		_position = transform.position;
        _nominalScale = transform.localScale;
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
            if(_hasAnimation) {
                transform.eulerAngles = Vector3.forward * (movingFactor * 360f * _rotationDirection);
                transform.localScale = Vector3.Lerp(_nominalScale, _nominalScale * .25f, EaseOutSine(movingFactor));
            }
            else {
                transform.eulerAngles = Vector3.forward;
                transform.localScale = _nominalScale;
            }
        }
        else if(_isHovering) {
            float angle = 2f * Mathf.PI * _angleFactor;
            _position = new Vector3(Mathf.Cos(angle) * 1.5f, Mathf.Sin(angle) * 1f, 0f);
            _angleFactor += Time.deltaTime * .5f;

            transform.position = _position + new Vector3(0f, Mathf.Sin(Time.time * 15f) * .1f, 0f);

        }
	}

    public void MoveTo(Vector3 destination, float duration, bool hasAnimation = false) {
        _rotationDirection = Random.value > .5f ? 1f : -1f;
        _hasAnimation = hasAnimation;
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

    public void MoveAway(float duration, bool hasAnimation = false) {
        MoveTo(transform.position + new Vector3(0f, 10f, 0f), duration, hasAnimation);
    }

    float EaseInOutSine(float t) {
        return (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
    }

    float EaseOutSine(float t) {
        return Mathf.Sin(t * Mathf.PI / 2f);
    }
}
