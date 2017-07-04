using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MasterSparkCodeBar : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Camera _camera;
#pragma warning restore 0649

    private Vector3 scaleMultVector;

	void Start()
	{
        if (_camera == null)
            _camera = transform.parent.GetComponent<Camera>();


        //transform.position = _camera.transform.position;
        //transform.rotation = _camera.transform.rotation;
        scaleMultVector = transform.lossyScale / _camera.orthographicSize;
	}
	
	void LateUpdate()
	{
        transform.localScale = scaleMultVector * _camera.orthographicSize;
        //Debug.Log(transform.localScale);

    }

    float getScale()
    {
        return transform.position.x;
    }

    void setScale(float scale)
    {

    }
}
