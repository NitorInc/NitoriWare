using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MasterSparkCodeBar : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Camera _camera;
#pragma warning restore 0649

    private Vector3 scaleMultVector;

	void Start()
	{
        if (_camera == null)
            _camera = transform.parent.GetComponent<Camera>();
        scaleMultVector = transform.lossyScale / _camera.orthographicSize;
	}
	
	void LateUpdate()
	{
        transform.localScale = scaleMultVector * _camera.orthographicSize;
    }

    float getScale()
    {
        return transform.position.x;
    }

    void setScale(float scale)
    {

    }
}
