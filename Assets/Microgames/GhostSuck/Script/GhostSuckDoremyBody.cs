using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckDoremyBody : MonoBehaviour {
    [SerializeField]
    private Sprite doremyleft;
    [SerializeField]
    private Sprite doremyright;
    [SerializeField]
    private Sprite doremycenter;
    [SerializeField]
    private Animator doremyanim;
    [SerializeField]
    private bool suck = false;
    private bool turn = false;


    private SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start () {
        Animator doremyanim = GetComponentInChildren<Animator>();
        doremyanim.enabled = (false);
    }
    


    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            suck = true;
        }
        else
            {
            suck = false;
        }

        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        if (cursorPosition.x > 3 && cursorPosition.y < cursorPosition.x)
        {
            if (suck == false)
                {

                
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (false);
                SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = doremyright;

            }
            else if (suck == true)
                {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (true);
                doremyanim.Play("doremyanimside");
            }
            if (turn != true)
            {
                turn = true;
                transform.Rotate(new Vector3(0, 180, 0));
            
            }
            


        }
        else if (cursorPosition.x < -3 && cursorPosition.y < -cursorPosition.x)
        {
            if (suck == false)
                {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (false);
                SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = doremyleft;
            }
           
            else if (suck == true)
                {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (true);
                doremyanim.Play("doremyanimside");
            }
            if (turn != false)
            {
                turn = false;
                transform.Rotate(new Vector3(0, 180, 0));
            
            }
        }
        else
        {
            if (suck == false)
                {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (false);
                SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                spriteRenderer.sprite = doremycenter;
            }
            else if (suck == true)
                {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (true);
                doremyanim.Play("doremyanimcentral");
            }
            if (turn != false)
            {
                turn = false;
                transform.Rotate(new Vector3(0, 180, 0));
               
            }

        }

    }
}
