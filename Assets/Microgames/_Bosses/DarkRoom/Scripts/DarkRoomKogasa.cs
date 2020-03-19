using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomKogasa : MonoBehaviour
{
    [SerializeField]
    private float revealDistance = 3f;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float lightActivateTime;
    [SerializeField]
    private float activationDelay = .6f;
    [SerializeField]
    private float scareDistance = -1f;

    private float lightActivateTimer;
    private float activationDelayTimer;
    private float vibrateX;
    private float vibrateY;
    private bool revealed;

	// Use this for initialization
	void Start ()
    {
        lightActivateTimer = lightActivateTime;
        activationDelayTimer = activationDelay;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (!revealed && transform.position.x < MainCameraSingleton.instance.transform.position.x + revealDistance)
        {
            rigAnimator.SetTrigger("Reveal");
            GetComponent<DarkRoomInstrumentDistance>().enabled = true;
            revealed = true;
        }
        if (revealed)
        {
            activationDelayTimer -= Time.deltaTime;
            if (transform.position.x < MainCameraSingleton.instance.transform.position.x + scareDistance)
            {
                rigAnimator.SetTrigger("Scare");
                DarkRoom_RenkoBehavior.instance.Fail();
            }
        }
        else
            activationDelayTimer = activationDelay;


        var t = 1f - (lightActivateTimer / lightActivateTime);
        rigAnimator.SetFloat("Shake", t);
    }


    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (!enabled)
            return;

        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light" && revealed && activationDelayTimer <= 0f)
        {
            lightActivateTimer -= Time.deltaTime;
            if (lightActivateTimer <= 0f)
            {
                rigAnimator.SetTrigger("Expose");
                GetComponent<DarkRoomInstrumentDistance>().enabled = false;
                enabled = false;
            }
        }

    }


    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (!enabled)
            return;

        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light")
        {
            lightActivateTimer = lightActivateTime;
        }

    }
}
