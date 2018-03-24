using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DatingSimMenuExpand : MonoBehaviour
{
    [SerializeField]
    private float targetYScale = 1f;
    public float TargetYScale { get { return targetYScale;} set { targetYScale = value; } }
    [SerializeField]
    private float scaleSpeed = 10f;
    public float ScaleSpeed { get { return scaleSpeed; } set { scaleSpeed = value; } }
    [SerializeField]
    private UnityEvent onExpanded;
    
	void Start ()
    {
        SetYScale(0f);
	}
	
	void Update ()
    {
        SetYScale(Mathf.MoveTowards(getYScale(), targetYScale, scaleSpeed * Time.deltaTime));
        if (getYScale() >= targetYScale)
        {
            onExpanded.Invoke();
            enabled = false;
        }
	}

    void SetYScale(float scale)
    {
        transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
    }

    float getYScale()
    {
        return transform.localScale.y;
    }
}
