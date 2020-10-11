using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceMarisa : MonoBehaviour {

	[SerializeField]
	float moveSpeed;
    [SerializeField]
    float moveAcc = 10f;
    [SerializeField]
    private float speedAngleMult = 2f;
    [SerializeField]
    private float speedSquashMult = .01f;
	[SerializeField]
	float yBound;
    [SerializeField]
    private int ringsRequired = 2;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private BroomRaceBackgroundSpeed bgSpeedComponent;
    [SerializeField]
    private BroomRaceRing[] ringComponents;
    [SerializeField]
    private float ringFailXDistance;
    [SerializeField]
    private float ringMovementCooldownTime = .3f;

    private int ringsHit = 0;
    private float currentSpeed = 0f;
    bool hasFailed;
    private float canMoveAfter = 0f;
    Vector3 initialScale;

    private void Start()
    {
        //transform.position = new Vector3(transform.position.x, Random.Range(-yBound, yBound), transform.position.x);
        initialScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("MicrogameTag1") && !hasFailed)
        {
            collision.GetComponent<BroomRaceRing>().activate();
            ringsHit++;

            rigAnimator.SetInteger("Rings", ringsHit);
            rigAnimator.SetTrigger("Ring");
            if (ringsHit == ringsRequired)
            {
                rigAnimator.SetTrigger("Victory");
                MicrogameController.instance.setVictory(true);
                moveSpeed = 0f;
            }

            speedAngleMult /= 2f;
            speedSquashMult /= 2f;

            canMoveAfter = Time.time + ringMovementCooldownTime;

            collision.enabled = false;
        }
    }

    void Fail()
    {
        rigAnimator.SetTrigger("Fail");
        moveSpeed = 0f;
        MicrogameController.instance.setVictory(false);
        for (int i = ringsHit + 1; i < ringsRequired; i++)
        {
            ringComponents[i].gameObject.SetActive(false);
        }
        hasFailed = true;
    }

    void Update()
    {
        var goalSpeedFactor = moveSpeed * Mathf.Min(bgSpeedComponent.SpeedMult, 1f);
        var goalSpeed = 0f;

        if (Time.time >= canMoveAfter)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                goalSpeed += goalSpeedFactor;
            if (Input.GetKey(KeyCode.DownArrow))
                goalSpeed -= goalSpeedFactor;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, goalSpeed, moveAcc * Time.deltaTime);
        if (currentSpeed != 0f)
        {
            transform.position += Vector3.up * currentSpeed * Time.deltaTime;
            if (transform.position.y > yBound)
                transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
            if (transform.position.y < -yBound)
                transform.position = new Vector3(transform.position.x, -yBound, transform.position.z);
        }
        transform.localEulerAngles = Vector3.forward * speedAngleMult * currentSpeed;

        var squashFactor = 1f + (currentSpeed * speedSquashMult);
        var newScale = initialScale;
        newScale.x = 1f / squashFactor;
        newScale.y = squashFactor;
        transform.localScale = newScale;


        if (ringsHit < ringsRequired
            && !hasFailed
            && transform.position.x > ringComponents[ringsHit].transform.position.x + ringFailXDistance)
                Fail();
        
        //if (Input.GetKeyDown(KeyCode.L))
        //    Fail();
    }
}
