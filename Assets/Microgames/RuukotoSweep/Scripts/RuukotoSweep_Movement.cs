using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuukotoSweep_Movement : MonoBehaviour {

    public Animator rigAnimator;

    public float leftMovementLimit;
    public float rightMovementLimit;
    public float upMovementLimit;
    public float downMovementLimit;

    public float horizontalMovementSpeed, verticalMovementSpeed;
    public Transform legTransform;

    public AudioClip victoryClip;

    private bool isInVictoryPose;

	// Use this for initialization
	void Start () {
        isInVictoryPose = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isInVictoryPose && MicrogameController.instance.session.BeatsRemaining >= .25f)
            moveCharacter();
        else
            disable();
    }

    void disable()
    {
        rigAnimator.SetInteger("Walk", 0);
        rigAnimator.SetFloat("WalkSpeed", 0f);
        enabled = false;
    }

    public void victory()
    {
        MicrogameController.instance.setVictory(true, true);
        Invoke("victoryPose", .1f);
        MicrogameController.instance.playSFX(victoryClip, panStereo: 0f);
    }

    void victoryPose()
    {
        rigAnimator.SetInteger("State", 1);
        isInVictoryPose = true;

        disable();
    }

    void moveCharacter()
    {
        Vector3 holdPosition = transform.position;
        manageVerticalMovement();
        manageHorizontalMovement();

        rigAnimator.SetInteger("Walk", transform.position.y != holdPosition.y ? 2 : (transform.position != holdPosition ? 1 : 0));
        rigAnimator.SetFloat("WalkSpeed", transform.position != holdPosition ? 1f: 0f);

        if (transform.position.x != holdPosition.x)
            legTransform.localScale = new Vector3(transform.position.x > holdPosition.x ? 1f : -1f, 1f, 1f);
    }

    void manageVerticalMovement()
    {
        float newPosition = float.NaN;
        if (Input.GetKey(KeyCode.DownArrow))
            newPosition = transform.position.y - verticalMovementSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.UpArrow))
            newPosition = transform.position.y + verticalMovementSpeed * Time.deltaTime;
        if (!float.IsNaN(newPosition))
        {
            var clampedPosition = Mathf.Clamp(newPosition, downMovementLimit, upMovementLimit);
            transform.position = new Vector2(transform.position.x, clampedPosition);
        }
    }

    void manageHorizontalMovement()
    {
        float newPosition = float.NaN;
        if (Input.GetKey(KeyCode.LeftArrow))
            newPosition = transform.position.x - horizontalMovementSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.RightArrow))
            newPosition = transform.position.x + horizontalMovementSpeed * Time.deltaTime;
        if (!float.IsNaN(newPosition))
        {
            var clampedPosition = Mathf.Clamp(newPosition, leftMovementLimit, rightMovementLimit);
            transform.position =  new Vector2(clampedPosition, transform.position.y);
        }
    }
}
