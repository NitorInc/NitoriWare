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

    private float activationTimer;
    private bool revealed;

	// Use this for initialization
	void Start ()
    {
        activationTimer = lightActivateTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (!revealed && transform.position.x < MainCameraSingleton.instance.transform.position.x + revealDistance)
        {
            rigAnimator.SetTrigger("Reveal");
            revealed = true;
        }
        if (revealed)
            activationDelay -= Time.deltaTime;
	}


    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (!enabled)
            return;

        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light" && revealed && activationDelay <= 0f)
        {
            activationTimer -= Time.deltaTime;
            if (activationTimer <= 0f)
            {
                rigAnimator.SetTrigger("Expose");
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
            activationTimer = lightActivateTime;
        }

    }
}
