using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMoney : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Vector2 speedBounds, torqueBounds, bounceVolumeSpeedBounds;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private Collider2D clickCollider;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip chingClip, bounceClip;
#pragma warning restore 0649

    private Vector2 lastVelocity;

	void Start()
	{
        _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), MathHelper.randomRangeFromVector(speedBounds));
        _rigidBody.AddTorque(MathHelper.randomRangeFromVector(torqueBounds) * (MathHelper.randomBool() ? 1f : -1f));
        lastVelocity = Vector2.zero;
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
        {
            animator.SetBool("Ching", true);
        }

        if (lastVelocity != Vector2.zero)
        {
            sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
            if ((Mathf.Sign(_rigidBody.velocity.x) == -Mathf.Sign(lastVelocity.x))
                || (Mathf.Sign(_rigidBody.velocity.y) == -Mathf.Sign(lastVelocity.y)))
            {
                float speed = _rigidBody.velocity.magnitude;
                sfxSource.PlayOneShot(bounceClip,
                    Mathf.Pow(Mathf.Lerp(0f, 1f,
                    ((speed - bounceVolumeSpeedBounds.x) / (bounceVolumeSpeedBounds.y - bounceVolumeSpeedBounds.y))),
                    .5f));
            }
        }

        lastVelocity = _rigidBody.velocity;
    }

    public void playChingSound()
    {
        sfxSource.PlayOneShot(chingClip, 1f);
    }
}
