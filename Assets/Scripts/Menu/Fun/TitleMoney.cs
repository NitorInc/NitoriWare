using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMoney : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Vector2 speedBounds, torqueBounds;
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

	void Start()
	{
        _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), MathHelper.randomRangeFromVector(speedBounds));
        _rigidBody.AddTorque(MathHelper.randomRangeFromVector(torqueBounds) * (MathHelper.randomBool() ? 1f : -1f));
	}
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
        {
            animator.SetBool("Ching", true);
        }
    }

    public void playChingSound()
    {
        sfxSource.PlayOneShot(chingClip, 1f);
    }
}
