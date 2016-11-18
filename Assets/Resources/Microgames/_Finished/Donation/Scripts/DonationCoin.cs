using UnityEngine;
using System.Collections;

public class DonationCoin : MonoBehaviour
{

	public float speed, minSpawnX, maxSpawnX, minStartTime, maxStartTime, slowHeight, fallHeight;

	private float startTime;

	public Rigidbody2D body;
	public bool isMainCoin;

	private Vector2 lastVelocity;

	public AudioSource bounceSource, grabSource;
	public AudioClip bounceClip;
	new public Collider2D collider;

	void Awake()
	{
		if (isMainCoin)
			reset();
	}


	public void reset ()
	{

		Transform coins = transform.parent;
		for (int i = 0; i < coins.childCount;coins.GetChild(i).GetComponent<DonationCoin>().resetBody(), i++);
		bounceSource.Stop();
	}

	void resetBody()
	{
		do
		{
			transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), 6f, transform.position.z);
		}

		while (Mathf.Abs(transform.position.x) < maxSpawnX / 3f);


		startTime = Random.Range(minStartTime, maxStartTime);
		body.velocity = Vector2.zero;

		transform.rotation = Quaternion.identity;
		body.angularVelocity = 0f;




		lastVelocity = new Vector2(0f, -1f);

	}
	
	void Update ()
	{
		if (startTime > 0f)
		{
			startTime -= Time.deltaTime;
			if (startTime <= 0f)
			{
				body.isKinematic = false;
				collider.enabled = true;
				startTime = 0f;
			}
		}	
		else if (transform.position.y <= slowHeight && transform.position.y > fallHeight)
		{
			//body.isKinematic = true;
			body.velocity = new Vector2(0f, speed * -1f);
		}
		else if (transform.position.y <= fallHeight)
		{
			body.velocity = new Vector2(0f, speed * -3f);
		}
		else
		{
			body.AddTorque(body.velocity.x * -1f);
		}


		if (body.velocity.y > 0f && lastVelocity.y <= 0f)
		{
			playBounceSound(body.velocity.y);
		}
		if ((body.velocity.x < 0f && body.velocity.x > 0f)
			|| (body.velocity.x > 0f && body.velocity.x < 0f))
		{
			playBounceSound(body.velocity.x);
		}

		lastVelocity = body.velocity;
	}

	void playBounceSound(float volume)
	{
		//source.pitch = Random.Range(.7f, .75f);
		bounceSource.pitch = Time.timeScale * .85f;

		volume = Mathf.Min(1f, volume);
		volume /= 2f;
		bounceSource.volume = volume;

		bounceSource.panStereo = getStereoPan();

		bounceSource.PlayOneShot(bounceClip);
	}

	float getStereoPan()
	{
		return (transform.position.x / (Camera.main.orthographicSize * 4f / 3f)) * .9f;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Reimu")
		{
			grabSource.pitch = .8f * Time.timeScale;
			grabSource.panStereo = getStereoPan();
			grabSource.Play();
			DonationReimu reimu = other.GetComponent<DonationReimu>();
			reimu.grabCoin(transform);
			grab();
		}
	}

	void grab()
	{
		transform.position = new Vector3(0f, -10f, transform.position.z);
		body.velocity = Vector2.zero;
	}
}
