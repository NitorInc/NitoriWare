using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindDrawer : MonoBehaviour {
    private bool _isLocked = true;
    public bool isLocked {
        get { return _isLocked; }
        set { _isLocked = value; }
    }

    private bool _isOpen = true;
    public bool isOpen {
        get { return _isOpen; }
        set { _isOpen = value; }
    }

    private CheeseFindController _controller;
    public CheeseFindController controller {
        get { return _controller; }
        set { _controller = value; }
    }

    private bool _isPulling = false;
    private float _pullFactor = 1f;
    private float _pullLimit = .5f;


	void Start() {
	}

    public void SetController(CheeseFindController controller) {
        _controller = controller;
    }
	
	void Update() {
        if(!_isPulling && _pullFactor > 0f) {
            if(!_isOpen) {
                _pullFactor -=  Time.deltaTime;
                if(_pullFactor < 0f)
                    _pullFactor = 0f;
            }

            Vector3 drawerPos = new Vector3(
                transform.parent.position.x,
                transform.parent.position.y - Mathf.Lerp(0f, _pullLimit, EaseInOutSine(_pullFactor)),
                transform.parent.position.z);
            transform.position = drawerPos;
        }

        float scaleFactor = .25f * _pullFactor + 1f;
		transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
	}

    void OnMouseDown() {
        if(_isLocked)
            return;

        _isPulling = true;

        //TODO: Click feedback (Sound + pop effect on the drawer)
    }

    void OnMouseUp() {
        if(_isLocked || !_isPulling)
            return;

        _isPulling = false;

        //TODO: If the pulling is not complete, revert to the init state, else lock open the drawer
    }

    void OnMouseDrag() {
        if(_isLocked || !_isPulling)
            return;

        float cursorY = CameraHelper.getCursorPosition().y;
        float deltaY = transform.parent.position.y - cursorY;

        _pullFactor = Mathf.Clamp(deltaY / _pullLimit, 0f, 1f);
        
        Vector3 drawerPos = new Vector3(
            transform.parent.position.x,
            transform.parent.position.y - Mathf.Lerp(0f, _pullLimit, EaseInOutQuart(_pullFactor)),
            transform.parent.position.z);
        transform.position = drawerPos;

        if(_pullFactor >= 1f) {
            _isPulling = false;
            _isLocked = true;
            _isOpen = true;
        }

        //TODO: Pulling animation
        //TODO: Lock open the drawer if the pull is complete
    }

    float EaseInOutSine(float t) {
        return (1f - Mathf.Cos(t * Mathf.PI)) / 2f;
    }

    float EaseInOutQuart(float t) {
        if(t < .5f)
            return 8f * t * t * t * t;
        else {
            float f = (t - 1f);
            return -8f * f * f * f * f + 1f;
        }
    }
}
