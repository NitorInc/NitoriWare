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

    [SerializeField]
    private float startFacingDelay = 0.5f;
    [SerializeField]
    private float startMovingDelay = 0.5f;
    [SerializeField]
    private Vector2 firstDirectionChangeTimeRange = new Vector2(1.1f, 1.4f);
    [SerializeField]
    private Vector2 directionChangeTimeRange = new Vector2(1.1f, 1.4f);
    [SerializeField]
    private Vector2 directionChangeAngleRange = new Vector2(150f, 210f);
    [SerializeField]
    private Vector2 xRange = new Vector2(-1f, 1f);
    [SerializeField]
    private Vector2 yRange = new Vector2(-3.5f, 3.5f);
    [SerializeField]
    private float rotateSpeed = 720f;
    [SerializeField]
    private float turnPauseTime = .25f;
    [SerializeField]
    private float minTurnTimeAfterBounce = .4f;
    [SerializeField]
    private float maxXForTurn = 3f;
    [SerializeField]
    private AudioClip bounceClip;
    [SerializeField]
    private AudioClip bounceTopClip;

    private ParticleSystem dust;
    private float lastBounceTime = -100f;

	//Direction of travel.
	private Vector2 trajectory = Vector2.up;

    private bool pause = true;

	SpriteRenderer myRenderer;
    Animator animator;
    private float goalRotation;

	//Sets trajectory to a random direction, and rotates sprite accordingly.

	//The animal moves faster the greater y is, because x-axis movement speed is constant.
	void Start () {
        dust = gameObject.GetComponent<ParticleSystem>();
        dust.Stop();
        animator = GetComponentInChildren<Animator>();
        transform.position = new Vector2(MathHelper.randomRangeFromVector(xRange), MathHelper.randomRangeFromVector(yRange));

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
            MicrogameController.instance.playSFX(bounceTopClip, panStereo: AudioHelper.getAudioPan(transform.position.x));
            //lastBounceTime = Time.time;
            //goalRotation = MathHelper.randomRangeFromVector(directionChangeAngleRange) - transform.rotation.eulerAngles.z;
        } else if (other.CompareTag ("MicrogameTag2") == true) {
			//Hit hand.
			trajectory.x *= -1;
            lastBounceTime = Time.time;

            other.transform.GetChild(transform.position.x < 0f ? 0 : 1)
                .GetComponent<Animator>().SetTrigger("Bounce");
            MicrogameController.instance.playSFX(bounceClip, panStereo: AudioHelper.getAudioPan(transform.position.x));
            //goalRotation = -transform.rotation.eulerAngles.z;
        } else if (other.CompareTag ("MicrogameTag3") == true)
        {
            MicrogameController.instance.setVictory(false);
        }

        goalRotation = trajectory.getAngle() - 90f;
        animator.transform.localEulerAngles = Vector3.forward * goalRotation;
    }

	

	//Move along trajectory.
	void Update () {
        if (pause == false)
        {
            transform.position = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            
        }

        goalRotation = trajectory.getAngle() - 90f;
        var angleDiff = MathHelper.getAngleDifference(animator.transform.eulerAngles.z, goalRotation);
        var frameDiff = Mathf.Sign(angleDiff) * Time.deltaTime * rotateSpeed;
        if (Mathf.Abs(angleDiff) <= Mathf.Abs(frameDiff))
            animator.transform.localEulerAngles = Vector3.forward * goalRotation;
        else
            animator.transform.localEulerAngles += Vector3.forward * frameDiff;

    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startFacingDelay);
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
        trajectory = newTrajectory;
        //Rotate sprite.
        goalRotation = trajectory.getAngle() - 90f;
        animator.transform.localEulerAngles = Vector3.forward * goalRotation;

        yield return new WaitForSeconds(startMovingDelay);

        pause = false;
        dust.Play();

    }

    IEnumerator ChangeDirection()
    {
        yield return new WaitForSeconds(MathHelper.randomRangeFromVector(firstDirectionChangeTimeRange));

        //pause = true;
        dust.Stop();
        //animator.SetBool("Rotate", true);
        //x becomes either 1 or -1, y becomes anything between -1 and 1.
        //float x = -trajectory.x;
        //float y = -trajectory.y;
        //Vector2 newTrajectory = new Vector2(x, y);

        yield return new WaitForSeconds(0.3f);

        while(true)
        {
            //var timeSinceLastBounce = Time.time - lastBounceTime;
            //if (timeSinceLastBounce < minTurnTimeAfterBounce)
            //    yield return new WaitForSeconds(minTurnTimeAfterBounce - timeSinceLastBounce);
            while (Mathf.Abs(transform.position.x) > maxXForTurn)
            {
                yield return new WaitForSeconds(0.05f);
            }

            //animator.SetBool("Rotate", false);
            //Rotate sprite.
            //transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.up, newTrajectory) * -x / Mathf.Abs(x));
            trajectory = MathHelper.getVector2FromAngle(trajectory.getAngle() + MathHelper.randomRangeFromVector(directionChangeAngleRange),
                trajectory.magnitude);
            //trajectory = newTrajectory;
            pause = true;
            yield return new WaitForSeconds(turnPauseTime);
            pause = false;
            dust.Play();
            yield return new WaitForSeconds(MathHelper.randomRangeFromVector(directionChangeTimeRange));
        }

    }


}
