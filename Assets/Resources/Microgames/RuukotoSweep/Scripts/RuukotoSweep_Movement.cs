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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (!MicrogameController.instance.getVictoryDetermined() && MicrogameTimer.instance.beatsLeft >= .5f)
            moveCharacter();
    }

    void moveCharacter()
    {
        Vector3 holdPosition = transform.position;
        manageVerticalMovement();
        manageHorizontalMovement();

        rigAnimator.SetInteger("Walk", transform.position.x != holdPosition.x ? 1 : (transform.position != holdPosition ? 2 : 0));
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
