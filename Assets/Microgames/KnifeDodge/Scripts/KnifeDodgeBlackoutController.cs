using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeBlackoutController : MonoBehaviour {
    public float targetAlpha
    {
        get;
        set;
    }
    public float fadeSpeed
    {
        get;
        set;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Debug.Log(gameObject.GetComponentsInChildren<SpriteRenderer>().Length   );
        foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, Mathf.Lerp(sr.color.a, targetAlpha, Time.deltaTime * fadeSpeed));
        }
    }

}
