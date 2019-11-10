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
	float yBound;
    [SerializeField]
    private int ringsRequired = 2;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private BroomRaceBackgroundSpeed bgSpeedComponent;

    private int rings = 0;
    private float currentSpeed = 0f;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(-yBound, yBound), transform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("MicrogameTag1"))
        {
            collision.GetComponent<BroomRaceRing>().activate();
            rings++;

            rigAnimator.SetInteger("Rings", rings);
            rigAnimator.SetTrigger("Ring");
            if (rings == ringsRequired)
            {
                rigAnimator.SetTrigger("Victory");
                MicrogameController.instance.setVictory(true);
                moveSpeed = 0f;
            }

            collision.enabled = false;
        }
    }

    void Update()
    {
        var goalSpeedFactor = moveSpeed * Mathf.Min(bgSpeedComponent.SpeedMult, 1f);
        var goalSpeed = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
            goalSpeed += goalSpeedFactor;
        if (Input.GetKey(KeyCode.DownArrow))
            goalSpeed -= goalSpeedFactor;

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
    }
}
