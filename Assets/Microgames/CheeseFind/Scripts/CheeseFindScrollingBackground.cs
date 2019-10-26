using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFindScrollingBackground : MonoBehaviour {
	private Vector3 _origin;
	private Vector3 _destination;
	private float _time = 0f;

	void Start () {
		_origin = transform.position + new Vector3(0f, -10f, 0f);
		_destination = transform.position;
	}
	
	void Update () {
		_time += Time.deltaTime * .5f;
		if(_time > 1f)
			_time -= 1f;
		transform.position = Vector3.Lerp(_origin, _destination, _time);
	}
}