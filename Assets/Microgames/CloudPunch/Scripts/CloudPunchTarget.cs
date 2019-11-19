using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPunchTarget : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.tag.Equals("MicrogameTag2"))
        {
            enabled = false;
            rigAnimator.SetTrigger("Hit");
        }
    }
}
