using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrowToSize : MonoBehaviour
{

    public UnityEvent onMaxSize;

#pragma warning disable 0649
    [SerializeField]
    private float goalScale = 1f, growSpeed;
    [SerializeField]
    public bool disableOnMaxSize;
#pragma warning restore 0649
    
	
	void Update()
	{
        float diff = growSpeed * Time.deltaTime;
        if (goalScale - transform.localScale.x <= diff)
        {
            transform.localScale = new Vector3(goalScale, goalScale, transform.localScale.z);
            onMaxSize.Invoke();
            if (disableOnMaxSize)
                enabled = false;
        }
        else
            transform.localScale += (Vector3)Vector2.one * diff;
	}
}
