using UnityEngine;
using System.Collections;

public class ChenFoodChen : MonoBehaviour
{

	public int leaps;

	public float attackTime, attackGrabTime, attackSlowSpeed;

	private float attackIn;


	public State state;

	public ChenFoodVibrate head, body;
	public ChenFoodFish fish;

	private int leapsLeft;
	private Vector2 attackGoal;
	private float attackAngle;
	private float attackSpeed, attackStartSpeed;
    public float approachXStart, approachXGoal, approachSpeed, approachStartTime, approachDuration;

	public AudioSource leapSource;
	public AudioClip[] leapClips;
	public AudioClip purrClip;
    public int approachSign;

	private Vector2 vibrateGoal;

	public enum State
	{
        Approach,
		Wait,
		Leap,
		Slow,
		Done
	}

	void Start()
	{
		reset();
	}

	public void reset()
	{
		fish.edible = false;
		fish.eaten = false;
		head.setSprite(false);


		leapsLeft = leaps;
		fish.updatePosition();
		fish.GetComponent<SpriteRenderer>().sprite = fish.sprite1;
		fish.lastPosition = fish.transform.position;

		fish.spriteSwitchIn = fish.spriteSwitchDistance;

        //do
        //{
        transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-3.5f, 3.5f));
        //}
        //while (((Vector2)transform.position - (Vector2)fish.transform.position).magnitude < 4f);
        transform.localRotation = Quaternion.identity;

        
		head.transform.localPosition = Vector3.zero;
		state = State.Approach;
		attackIn = attackTime;
		head.vibrateOn = true;
		body.vibrateOn = true;
		head.resetVibrate();


		Cursor.visible = false;

        approachSign = MathHelper.randomBool() ? -1 : 1;
        transform.position = new Vector3(approachSign * approachXStart, transform.position.y, transform.position.z);

        if (fish.transform.position.x < transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        leapSource.Stop();
	}


	void Update()
	{
        

        if (state == State.Approach)
        {
            if (approachStartTime > 0f)
            {
                approachStartTime -= Time.deltaTime;
            }
            else if (approachDuration > 0f)
            {
                var goalPosition = new Vector3(approachSign * approachXGoal, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, goalPosition,
                    approachSpeed * Time.deltaTime * (goalPosition - transform.position).magnitude);

                approachDuration -= Time.deltaTime;
                if (approachDuration <= 0f)
                    state = State.Wait;
            }
        }
		else if (state == State.Wait)
		{
			if (attackIn < .2f)
			{
				fish.edible = true;
			}

			if (fish.eaten)
				startAttack();

			if (fish.transform.position.x < transform.position.x)
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
			else
				transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

			attackIn -= Time.deltaTime;
			if (attackIn < 0f)
			{
				startAttack();
			}
		}
		else if (state == State.Leap)
		{
			if (leaps == 3 && leapsLeft == 1)
				rotate();
			Vector2 goalVector = (attackGoal - (Vector2)transform.position);
			float diff = attackSpeed * Time.deltaTime;
			if (diff >= goalVector.magnitude)
			{
				transform.position = new Vector3(attackGoal.x, attackGoal.y, transform.position.z);
				state = State.Slow;
			}
			else
			{

				transform.position += (Vector3)goalVector.resize(attackSpeed * Time.deltaTime);

			}
		}
		else if (state == State.Slow)
		{
			if (leapsLeft > 1 && !fish.eaten)
				attackSpeed -= attackSlowSpeed * Time.deltaTime * attackStartSpeed;

			if (leaps == 3 && leapsLeft == 1)
				rotate();

			if (attackSpeed <= 0f)
			{
				leapsLeft--;
				if (leapsLeft > 0)
				{
					state = State.Wait;
					attackIn = .15f;
				}
				else
					state = State.Done;
			}
			else
			{
				transform.position += (Vector3)MathHelper.getVector2FromAngle(attackAngle * Mathf.Rad2Deg, attackSpeed * Time.deltaTime);
			}
		}
		else
		{
			if (fish.eaten)
				startAttack();
		}

		body.setSprite(state == State.Leap|| state == State.Slow);
		head.setSprite(fish.eaten);

		leapSource.panStereo = AudioHelper.getAudioPan(transform.position.x) * .85f;
	}

	void LateUpdate()
	{
		fish.eyes.transform.localPosition = new Vector3(0f, 0f, (transform.position.x > fish.transform.position.x) ? .01f: -.01f);
	}

	void startAttack()
	{
		fish.updatePosition();

		state = State.Leap;
		attackGoal = (Vector2)fish.transform.position;
		attackSpeed = (attackGoal - (Vector2)transform.position).magnitude / attackGrabTime;
		if (fish.eaten)
			attackSpeed /= 2f;
		attackStartSpeed = attackSpeed;

		attackAngle = (attackGoal - (Vector2)transform.position).getAngle() * Mathf.Deg2Rad;

		leapSource.PlayOneShot(leapClips[leaps - leapsLeft]);
	}

	void rotate()
	{
		float spin = (Time.deltaTime * 15f);
		if (transform.localScale.x < 0f)
			spin *= -1f;

		transform.localRotation = Quaternion.Euler(0f, 0f, ((transform.localRotation.eulerAngles.z * Mathf.Deg2Rad) + spin) * Mathf.Rad2Deg);
	}

}