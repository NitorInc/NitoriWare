using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuControlPiece : MonoBehaviour
{
    [SerializeField]
    private float appearSpeed = 2f;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer() => spriteRenderer;
    [SerializeField]
    private SpriteMask mask;
    public SpriteMask GetMask() => mask;

    bool fadingIn = false;
    
	void Start ()
    {

    }

    private void Update()
    {
        if (fadingIn)
        {
            var c = spriteRenderer.color;
            c.a += appearSpeed * Time.deltaTime;
            spriteRenderer.color = c;
            if (c.a >= 1f)
                fadingIn = false;
        }
    }


    public void SetAppearTime(float time)
    {
        var c = spriteRenderer.color;
        c.a = 0f;
        spriteRenderer.color = c;
        fadingIn = false;
        CancelInvoke();
        Invoke("StartAppearing", time);
    }

    void StartAppearing()
    {
        fadingIn = true;
    }
}
