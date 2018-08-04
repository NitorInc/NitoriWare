using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeWhiteOutController : MonoBehaviour {
    enum WhiteOutState { WAITING, FLASHING, FADEOUT }
    int state;
    float fadeSpeed;
    float fadeAlpha;
    float expansionSpeed;
    float timeUntilFade;

    public void DoFlash()
    {
        if (state == (int) WhiteOutState.WAITING)
            this.state = (int) WhiteOutState.FLASHING;
    }
    // Use this for initialization
    void Start () {
        state = (int) WhiteOutState.WAITING;
        fadeSpeed = 0;
        fadeAlpha = 0;
        expansionSpeed = 5f;
        timeUntilFade = 0.2f;
    }
	
	// Update is called once per frame
	void Update () {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color srCol = sr.color;
        Vector3 currentSize = sr.transform.localScale;
        switch (state)
        {
            case (int) WhiteOutState.WAITING:
                break;
            case (int) WhiteOutState.FLASHING:
                timeUntilFade -= Time.deltaTime;
                if (timeUntilFade < 0)
                {
                    state = (int)WhiteOutState.FADEOUT;
                }

                fadeAlpha = 100f;
                fadeSpeed = 10.0f;

                sr.color = new Color(srCol.r, srCol.g, srCol.b, Mathf.Lerp(srCol.a, fadeAlpha, Time.deltaTime * fadeSpeed));
                sr.transform.localScale = new Vector3(
                    Mathf.Lerp(currentSize.x, 12, expansionSpeed * Time.deltaTime), 
                    Mathf.Lerp(currentSize.y, 12, expansionSpeed * Time.deltaTime), 
                    Mathf.Lerp(currentSize.z, 12, expansionSpeed * Time.deltaTime));
                break;
            case (int) WhiteOutState.FADEOUT:
                fadeAlpha = 0;
                fadeSpeed = 10.0f;
                sr.color = new Color(srCol.r, srCol.g, srCol.b, Mathf.Lerp(srCol.a, fadeAlpha, Time.deltaTime * fadeSpeed));
                break;
        }
    }
}
