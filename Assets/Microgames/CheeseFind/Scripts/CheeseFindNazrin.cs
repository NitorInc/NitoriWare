using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindNazrin : MonoBehaviour {
    [Header("Start Position")]
    [SerializeField]
    private Vector3 startPosition;

    [Header("End Position")]
    [SerializeField]
    private Vector3 endPosition;

    private bool _isMoving = false;
    private float _movingTime = 0f;
    private float _movingDuration = 1f;

    private GameObject _success8Rig, _success4Rig, _success3Rig, _failureRig;

	void Start () {
		transform.position = startPosition;
        _success8Rig = transform.Find("Success8").gameObject;
        _success4Rig = transform.Find("Success4").gameObject;
        _success3Rig = transform.Find("Success3").gameObject;
        _failureRig = transform.Find("Failure").gameObject;
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

    public void Activate(bool isVictorious, int score) {
        if(isVictorious) {
            if(score <= 3)
                _success3Rig.SetActive(true);
            else if(score <= 4)
                _success4Rig.SetActive(true);
            else
                _success8Rig.SetActive(true);
        }
        else
            _failureRig.SetActive(true);
        _isMoving = true;
    }
}
