using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectAfterTime : MonoBehaviour
{

    [SerializeField]
    private float _timer;
    public float timer { get { return _timer; } set { _timer = value; } }
    [SerializeField]
    private GameObject _targetObject;
    public GameObject targetObject { get { return _targetObject; } set { _targetObject = value; } }
    
	void Update ()
    {
        if (_timer > 0f)
        {
            _timer = Mathf.MoveTowards(_timer, 0f, Time.deltaTime);
            if (_timer <= 0f)
                enable();
        }
	}

    void enable()
    {
        targetObject.SetActive(true);
    }
}
