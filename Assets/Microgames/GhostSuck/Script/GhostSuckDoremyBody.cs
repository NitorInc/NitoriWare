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
    private AudioClip vacuumpersist;
    [SerializeField]
    private bool suck = true;
    private bool turn = false;
    private bool vacuumsoundready = true;


    private SpriteRenderer spriteRenderer;
    // Use this for initialization
    void Start () {
        Animator doremyanim = GetComponentInChildren<Animator>();
        doremyanim.enabled = (false);
    }

    void playVacuum()
    {
        MicrogameController.instance.playSFX(vacuumpersist, volume: 0.15f, pitchMult: 1.5f, panStereo: AudioHelper.getAudioPan(transform.position.x));
        Invoke("ResetVacuum", .65f);
    }
    void ResetVacuum()
    {
        vacuumsoundready = true;
    }

    // Update is called once per frame
    void Update() {
            if(vacuumsoundready == true)
            {
                vacuumsoundready = false;
                playVacuum();
            }
        //determines rotation and what animations or sprites to show based on where mouse is and whether mouse is clicked or not
        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        if (cursorPosition.x > 2 && cursorPosition.y < cursorPosition.x + 2)
        {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.Play("doremyanimside");
            if (turn != true)
            {
                turn = true;
                transform.Rotate(new Vector3(0, 180, 0));
            
            }
        }
        else if (cursorPosition.x < -2 && cursorPosition.y < -cursorPosition.x + 2)
        {

                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.Play("doremyanimside");
            if (turn != false)
            {
                turn = false;
                transform.Rotate(new Vector3(0, 180, 0));
            
            }
        }
        else
        {
                Animator doremyanim = GetComponentInChildren<Animator>();
                doremyanim.enabled = (true);
                doremyanim.Play("doremyanimcentral");
            if (turn != false)
            {
                turn = false;
                transform.Rotate(new Vector3(0, 180, 0));
               
            }

        }

    }
}
