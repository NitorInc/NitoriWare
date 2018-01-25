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
  new public Collider2D collider;

  private bool outOfPlay;

  void Awake()
  {
    if (isMainCoin)
      reset();
  }

  void Start()
  {
    outOfPlay = false;
    DonationReimu.coinsInPlay++;
  }

  public void reset()
  {
    Transform coins = transform.parent;
    for (int i = 0; i < coins.childCount; coins.GetChild(i).GetComponent<DonationCoin>().resetBody(), i++) ;
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

    lastVelocity = Vector2.down;
  }

  void Update()
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
    bounceSource.pitch = Time.timeScale * .85f;

    volume = Mathf.Min(1f, volume);
    volume /= 2f;
    bounceSource.volume = volume * PrefsHelper.getVolume(PrefsHelper.VolumeType.SFX);

    bounceSource.panStereo = getStereoPan();

    bounceSource.PlayOneShot(bounceClip);
  }

  float getStereoPan() => (transform.position.x / (Camera.main.orthographicSize * 4f / 3f)) * .9f;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.name == "Reimu")
    {
      outOfPlay = true;
      grabSource.clip = grabClips[Random.Range(0, grabClips.Length)];
      grabSource.pitch = Time.timeScale;
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
