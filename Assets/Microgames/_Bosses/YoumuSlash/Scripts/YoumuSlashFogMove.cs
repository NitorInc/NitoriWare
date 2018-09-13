using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashFogMove : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float WrapDistance = 10f * (4f / 3f);
    
	
	void Update ()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;
        if (Mathf.Abs(transform.localPosition.x) >= WrapDistance)
            transform.localPosition += Vector3.left * Mathf.Sign(speed) * WrapDistance * 2f;
	}
}
