using UnityEngine;
using System.Collections;

public class PotionIngredient : MonoBehaviour
{
	public float speedToPot;

	public PotionPot pot;
	public SpriteRenderer spriteRenderer;
	public AudioClip carryClip, dropClip;
	public float soundFrequency;

	public Collider2D theCollider;

	private Rigidbody2D rigidThing;
	private Vector2 grabOffset;
	private AudioSource _audioSource;
	private float soundDistance;
	private Vector2 lastPosition;

	public State state;
	public enum State
	{
		Idle,
		Grabbed,
		Used,
		SnapBack
	}

	void Awake ()
	{
		theCollider = GetComponent<Collider2D>();
		rigidThing = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();
	}

	void Update ()
	{
		switch(state)
		{
			case(State.Idle):
				{
					if (pot.state == PotionPot.State.Default && Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(theCollider))
					{
						rigidThing.isKinematic = true;
						grabOffset = (Vector2)(transform.position - CameraHelper.getCursorPosition());
						snapToMouse();
						pot.moveToFront(this);
						_audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
						_audioSource.PlayOneShot(carryClip);
						lastPosition = (Vector2)CameraHelper.getCursorPosition();
						state = State.Grabbed;
					}
				}
				break;
			case (State.Grabbed):
				updateSound();
				if (pot.state != PotionPot.State.Default || !Input.GetMouseButton(0))
				{
					rigidThing.isKinematic = false;
					rigidThing.velocity = Vector2.zero;
					soundDistance = 0f;
					state = State.Idle;
				}
				else
					snapToMouse();
				break;
			case (State.Used):
				rigidThing.bodyType = RigidbodyType2D.Static;
				transform.moveTowards(pot.transform.position + new Vector3(0f, -.5f, 0f), speedToPot);
				break;
			default:
				break;
		}

	}

	void updateSound()
	{
		_audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
		soundDistance += Mathf.Pow(((Vector2)CameraHelper.getCursorPosition() - lastPosition).magnitude, 1f/3f);
		if (soundDistance >= soundFrequency)
		{
			_audioSource.PlayOneShot(carryClip);
			soundDistance -= soundFrequency;
		}

		lastPosition = (Vector2)CameraHelper.getCursorPosition();
	}

	void snapToMouse()
	{
		Vector3 position = CameraHelper.getCursorPosition();
		transform.position = new Vector3(position.x, position.y, transform.position.z) + (Vector3)grabOffset;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		collision(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		collision(other);
	}

	void collision(Collider2D other)
	{
		if (state == State.Idle && other.name == "Front" && pot.state == PotionPot.State.Default)
		{
			state = State.Used;
			rigidThing.isKinematic = true;
			pot.addIngredient(this);
			//_audioSource.panStereo = 0f;
			_audioSource.PlayOneShot(dropClip);
		}
	}
}
