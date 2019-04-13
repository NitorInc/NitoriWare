using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindVictoryScreen : MonoBehaviour {
    [Header("Start Position")]
    [SerializeField]
    private Vector3 startPosition;

    [Header("End Position")]
    [SerializeField]
    private Vector3 endPosition;

    private bool _isMoving = false;
    private float _movingTime = 0f;
    private float _movingDuration = 1f;

	void Start () {
		transform.position = startPosition;
	}
	
	void Update () {
		if(_isMoving) {
            _movingTime += Time.deltaTime;
            float movingFactor = _movingTime / _movingDuration;
            if(_movingTime > _movingDuration) {
                movingFactor = 1f;
                _isMoving = false;
            }
		    transform.position = Vector3.Lerp(startPosition, endPosition, EaseOutElastic(movingFactor));
        }
	}

    float EaseOutElastic(float t) {
        return Mathf.Sin(-13f * (Mathf.PI / 2f) * (t + 1)) * Mathf.Pow(2f, -10f * t) + 1f;
    }

    public void Activate() {
        _isMoving = true;
    }
}
