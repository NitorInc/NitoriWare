using UnityEngine;
using System.Collections;

public class DonationCoin : MonoBehaviour
{

	public float speed, minSpawnX, maxSpawnX, minStartTime, maxStartTime, slowHeight, fallHeight;

	private float startTime, outOfPlayY = -4.2f;

	public Rigidbody2D body;
	public bool isMainCoin;

	private Vector2 lastVelocity;

	public AudioSource bounceSource, grabSource;
	public AudioClip bounceClip;
	public AudioClip[] grabClips;
	private Collider2D coinCollider;

	private bool outOfPlay;

    private const float ZeroTimer = .35f;
    private float zeroTime = 0f;
    private Collider2D lastTouchedNeighbor;

	void Awake()
	{
        coinCollider = GetComponent<Collider2D>();
		if (isMainCoin)
			reset();
	}

	void Start()
	{
		outOfPlay = false;
		DonationReimu.coinsInPlay++;
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


        startTime = Random.Range(minStartTime + .01f, maxStartTime);
		body.velocity = Vector2.zero;

		transform.rotation = Quaternion.identity;
		body.angularVelocity = 0f;




		lastVelocity = Vector2.down;

	}
	
	void Update ()
	{
		if (startTime > 0f)
		{
			startTime -= Time.deltaTime;
			if (startTime <= 0f)
			{
				body.isKinematic = false;
				coinCollider.enabled = true;
				startTime = 0f;
			}
		}	
		else if (transform.position.y <= slowHeight && transform.position.y > fallHeight)
		{
			body.velocity = Vector2.down * speed;
		}
		else if (transform.position.y <= fallHeight)
		{
			body.velocity = Vector2.down * speed * 3f;
		}
		else
		{
			body.AddTorque(-body.velocity.x);
		}
        
        float zeroThreshold = .01f;
        if (!body.isKinematic
            && MathHelper.Approximately(Mathf.Abs(body.velocity.x) + Mathf.Abs(body.velocity.y), 0f, zeroThreshold))
        {
            zeroTime += Time.deltaTime;
            if (zeroTime >= ZeroTimer && lastTouchedNeighbor != null)
            {
                Physics2D.IgnoreCollision(coinCollider, lastTouchedNeighbor);
                zeroTime = 0f;
            }
        }
        else
            zeroTime = 0f;


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

		if (!outOfPlay && transform.position.y < outOfPlayY)
		{
			outOfPlay = true;
			DonationReimu.coinsInPlay--;
		}
	}

	void playBounceSound(float volume)
	{
		bounceSource.pitch = .85f;

		volume = Mathf.Min(1f, volume);
		volume /= 2f;
		bounceSource.volume = volume;

		bounceSource.panStereo = getStereoPan();

		bounceSource.PlayOneShot(bounceClip);
	}

	float getStereoPan()
	{
		return (transform.position.x / (MainCameraSingleton.instance.orthographicSize * 4f / 3f)) * .9f;
	}

    private void OnCollisionEnter2D(Collision2D collision) => coinCollision(collision);

    private void OnCollisionStay2D(Collision2D collision) => coinCollision(collision);

    void coinCollision(Collision2D collision)
    {
        if (collision.collider.name.Contains("Coin"))
            lastTouchedNeighbor = collision.collider;
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Reimu")
		{
			outOfPlay = true;
			grabSource.clip = grabClips[Random.Range(0, grabClips.Length)];
			grabSource.pitch = 1f;
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
