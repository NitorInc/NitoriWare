using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasenPetsAnimal : MonoBehaviour {

	[Header("Speed")]
	[SerializeField]
	private float speed = 1f;

    [Header("Is Eagle?")]
    [SerializeField]
    private bool isEagle = false;


	//Direction of travel.
	private Vector2 trajectory = Vector2.up;

    private bool pause = true;


	SpriteRenderer myRenderer;
    Animator animator;


	//Sets trajectory to a random direction, and rotates sprite accordingly.

	//The animal moves faster the greater y is, because x-axis movement speed is constant.
	void Start () {
        animator = GetComponentInChildren<Animator>();
        transform.position = new Vector2(Random.Range(-1f, 1f), Random.Range(-3.5f, 3.5f));

        StartCoroutine(StartDelay());


        if(isEagle == true)
        {
            StartCoroutine(ChangeDirection());
        }

		
	}

	//Bounce off walls.
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("MicrogameTag1") == true) {
			//hit ceiling or floor.
			trajectory.y *= -1;
			transform.rotation = Quaternion.Euler (0, 0, 180 - transform.rotation.eulerAngles.z);
		} else if (other.CompareTag ("MicrogameTag2") == true) {
			//Hit hand.
			trajectory.x *= -1;
			transform.rotation = Quaternion.Euler (0, 0, -transform.rotation.eulerAngles.z);
		} else if (other.CompareTag ("MicrogameTag3") == true)
        {
            MicrogameController.instance.setVictory(false);
        }
	}

	

	//Move along trajectory.
	void Update () {
        if (pause == false)
        {
            transform.position = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
        }

	}

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(0.5f);
        //x becomes either 1 or -1, y becomes anything between -1 and 1.
        int x = 1;
        float y = Random.value;

        animator.SetBool("Pause", false);
        switch (Random.Range(0, 4))
        {
            case 0:
                break;
            case 1:
                x *= -1;
                break;
            case 2:
                y *= -1;
                break;
            case 3:
                x *= -1;
                y *= -1;
                break;
        }
        Vector2 newTrajectory = new Vector2(x, y);
        //Rotate sprite.
        transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(trajectory, newTrajectory) * -x / Mathf.Abs(x));
        trajectory = newTrajectory;

        yield return new WaitForSeconds(0.5f);

        pause = false;

    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(1.4f);

        pause = true;

        //x becomes either 1 or -1, y becomes anything between -1 and 1.
        float x = -trajectory.x;
        float y = Random.value;
        Vector2 newTrajectory = new Vector2(x, y);
        //Rotate sprite.
        transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, newTrajectory) * -x / Mathf.Abs(x));
        trajectory = newTrajectory;

        yield return new WaitForSeconds(0.3f);
        pause = false;

    }


}
