using UnityEngine;
using System.Collections;

public class SlingshotAmmo : MonoBehaviour
{


	public float flingSpeedMult, flingGravity;

	public float brokenRotateSpeed, brokenGravity, hitShake;
	public Vector2 brokenVelocity;


	public Transform head, slingshotBase;
	public Collider2D collide;
	public Vector2 cursorOffset;
	public Animator animator;
	public SlingshotTarget target;
	public SlingshotString leftString, rightString;
	public Transform backString;

	new public AudioSource audio;
	public AudioClip launchClip, hitClip, breakClip;

	public State state;

	//private float flingSpeed, flingAngle;
	private Rigidbody2D rigidThing;

	private Vector3 initialPosition, initialScale;



	public enum State
	{
		Waiting,
		Grabbed,
		Flinging,
		Flung,
		Broken
	}

	public void reset()
	{

		if (collide == null)
			collide = GetComponent<Collider2D>();

		animator.enabled = true;
		state = State.Waiting;
		transform.position = initialPosition;
		head.GetComponent<Collider2D>().enabled = false;

		leftString.gameObject.SetActive(true);
		rightString.gameObject.SetActive(true);

		rigidThing.velocity = Vector2.zero;
		rigidThing.gravityScale = 0f;
		rigidThing.isKinematic = true;
		transform.rotation = Quaternion.identity;

		target.reset();

		setSlingshotAngle(0f);

		backString.gameObject.SetActive(true);

		audio.pitch = audio.volume = 1f;
		audio.Stop();

	}

	void Awake ()
	{
		initialPosition = transform.position;
		initialScale = head.transform.localScale;
		rigidThing = GetComponent<Rigidbody2D>();

	}

	void Start()
	{
		reset();
	}
	
	void Update ()
	{
		switch(state)
		{
			case(State.Waiting):
				if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(collide))
				{
					state = State.Grabbed;

					cursorOffset = (Vector2)transform.position - (Vector2)CameraHelper.getCursorPosition();
					snapToMouse();
					animator.enabled = false;
					head.transform.localScale = initialScale;

					backString.gameObject.SetActive(false);

					audio.Play();
					audio.pitch = getStretchPitch();
					updateAudioPan();
				}
				break;
			case(State.Grabbed):
				snapToMouse();
				audio.pitch = getStretchPitch();
				updateAudioPan();
				Vector2 diff = (Vector2)initialPosition - (Vector2)transform.position;
				transform.rotation = Quaternion.Euler(0f, 0f,
					.5f * MathHelper.getVectorAngle2D(diff)
					* Mathf.Lerp(0f, 1f, Mathf.Abs(diff.y))
					* Mathf.Lerp(0f, 1f, diff.x));
				if (!Input.GetMouseButton(0))
				{	
					audio.Stop();
					//audio.pitch = Time.timeScale;
					if (diff.magnitude > 0f)
					{
						rigidThing.isKinematic = false;
						rigidThing.velocity = diff * flingSpeedMult;
						rigidThing.gravityScale = 0f;
						state = State.Flinging;


						updateAudioPan();
						audio.pitch -= .5f * Time.timeScale;
						audio.volume = (getStretchPitch() / Time.timeScale) - .75f;
						audio.PlayOneShot(launchClip);

						head.GetComponent<Vibrate>().vibrateOn = false;
					}
					else
					{
						state = State.Waiting;
						animator.enabled = true;
					}


				}
				else if (transform.position.x > 3.3f)
				{
					state = State.Broken;
					rigidThing.isKinematic = false;
					rigidThing.velocity = brokenVelocity;
					rigidThing.gravityScale = brokenGravity;
					MicrogameController.instance.setVictory(false, true);

					audio.Stop();

					updateAudioPan();
					audio.volume = 1f;
					audio.pitch = Time.timeScale;
					audio.PlayOneShot(breakClip);
				}
				tiltSlingShot();
				break;
			case(State.Flinging):
				Vector2 originDiff = (Vector2)initialPosition - (Vector2)transform.position;
				tiltSlingShot();

				if (originDiff.magnitude < rigidThing.velocity.magnitude * 1.5f * Time.deltaTime)
				{
					leftString.gameObject.SetActive(false);
					rightString.gameObject.SetActive(false);
					rigidThing.gravityScale = flingGravity;
					state = State.Flung;
					setSlingshotAngle(0f);

					backString.gameObject.SetActive(true);
				}
				//transform.Translate((Vector3)MathHelper.getVectorFromAngle(flingAngle, flingSpeed * Time.deltaTime));
				break;
			case(State.Flung):
				updateAudioPan();
				if (!MicrogameController.instance.getVictory() && rigidThing.velocity.x > 0f)
					transform.rotation = Quaternion.Euler(0f, 0f, MathHelper.getVectorAngle2D(rigidThing.velocity) * .5f);
				break;
			case(State.Broken):
				rigidThing.MoveRotation(rigidThing.rotation + (brokenRotateSpeed * 5f * Time.deltaTime));
				break;
			default:
				break;
		}
	}

	void updateAudioPan()
	{
		audio.panStereo = (transform.position.x / (Camera.main.orthographicSize * 4f / 3f)) * .8f;
	}

	float getStretchPitch()
	{
		float diff = ((Vector2)(transform.position - initialPosition)).magnitude;

		diff /= 10f;

		diff = Mathf.Min(.5f, diff);

		return (1f + diff) * Time.timeScale;
	}

	void tiltSlingShot()
	{
		setSlingshotAngle((initialPosition.x - transform.position.x) * .01f);
		//setSlingshotAngle(((Vector2)initialPosition - (Vector2)transform.position).magnitude * .01f
		//	* (transform.position.x < initialPosition.x ? 1f: -1f));
	}

	void setSlingshotAngle(float angle)
	{
		slingshotBase.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (state == State.Flung && other.name == "Target")
		{
			target.rigidThing.isKinematic = false;
			target.rigidThing.gravityScale = target.hitGravity;
			target.rigidThing.velocity = target.hitVelocity;
			target.hit = true;
			target.hitParticles.Play();
			target.hitParticles.transform.parent = transform.parent;

			state = State.Broken;
			rigidThing.isKinematic = false;
			rigidThing.velocity = brokenVelocity * 1.5f;
			rigidThing.gravityScale = 0f;

			updateAudioPan();
			audio.volume = 1f;
			audio.pitch = Time.timeScale;
			audio.PlayOneShot(hitClip);

			CameraShake.instance.setScreenShake(hitShake);

			MicrogameController.instance.setVictory(true, true);
		}
	}

	void snapToMouse()
	{
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z) + (Vector3)cursorOffset;
	}
}
