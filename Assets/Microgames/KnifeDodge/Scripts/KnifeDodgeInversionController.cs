using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeInversionController : MonoBehaviour {
    public float invertFilterAlpha
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
            Color srCol = sr.GetComponent<SpriteRenderer>().color;
            sr.GetComponent<SpriteRenderer>().color = new Color(srCol.r, srCol.g, srCol.b, Mathf.Lerp(srCol.a, invertFilterAlpha, Time.deltaTime * fadeSpeed));
        }
    }

}
