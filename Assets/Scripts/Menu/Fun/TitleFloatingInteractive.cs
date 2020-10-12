using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFloatingInteractive : MonoBehaviour
{
    public TitleInteractableSpawner spawner;
    public Vector2 lastVelocity;

#pragma warning disable 0649
    [SerializeField]
    private float startSpeed, lifetime, escapeSpeed;
    [SerializeField]
    private Vector2 floatTowardsBounds, bounceVolumeSpeedBounds;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private Collider2D wallHitCollider;
    [SerializeField]
    AudioSource sfxSource;
    [SerializeField]
    AudioClip bounceClip;
#pragma warning restore 0649

    private float colliderExtent;
    private int startTrailBuffer = 3;
    private TrailRenderer trail;

    void Start()
	{
        colliderExtent = Mathf.Max(wallHitCollider.bounds.extents.x, wallHitCollider.bounds.extents.y);
        wallHitCollider.enabled = false;

        Vector2 goal = new Vector2(Random.Range(-floatTowardsBounds.x, floatTowardsBounds.x),
            Random.Range(-floatTowardsBounds.y, floatTowardsBounds.y));
        _rigidBody.velocity = (goal - (Vector2)transform.localPosition).resize(startSpeed);
        lastVelocity = _rigidBody.velocity;

        trail = GetComponentInChildren<TrailRenderer>();
        if (trail != null)
            trail.enabled = false;
	}

    void LateUpdate()
    {
        if (trail != null && startTrailBuffer > 0)
        {
            startTrailBuffer--;
            if (startTrailBuffer <= 0)
            {
                trail.enabled = true;
                trail.Clear();
            }
        }

        if (!canStayActive())
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            Vector2 escapeVelocity = MathHelper.getVector2FromAngle(
                ((Vector2)(transform.position - MainCameraSingleton.instance.transform.position)).getAngle(), escapeSpeed);
            transform.position += (Vector3)escapeVelocity * Time.deltaTime;
            if (CameraHelper.isObjectOffscreen(transform, 10f))
                Destroy(gameObject);
            return;
        }
        else if (!wallHitCollider.enabled && !CameraHelper.isObjectOffscreen(transform,
            -colliderExtent))
        {
            wallHitCollider.enabled = true;
        }

        if (lifetime > 0f)
        {   
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
                setIgnoreWalls(true);
        }
        
        if (lastVelocity != Vector2.zero)
        {
            sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
            if ((Mathf.Sign(_rigidBody.velocity.x) == -Mathf.Sign(lastVelocity.x))
                || (Mathf.Sign(_rigidBody.velocity.y) == -Mathf.Sign(lastVelocity.y))
                || Mathf.Abs(_rigidBody.velocity.magnitude - lastVelocity.magnitude) > bounceVolumeSpeedBounds.x)
            {
                float speed = _rigidBody.velocity.magnitude;
                float volume = Mathf.Pow(Mathf.Lerp(.5f, 1f,
                    ((speed - bounceVolumeSpeedBounds.x) / (bounceVolumeSpeedBounds.y - bounceVolumeSpeedBounds.x))),
                    1f);
                if (volume > .5f && !float.IsNaN(volume))
                {
                    sfxSource.pitch = 1f;
                    sfxSource.PlayOneShot(bounceClip, volume);
                }
            }
        }
        lastVelocity = _rigidBody.velocity;

        if (CameraHelper.isObjectOffscreen(transform, 10f))
            Destroy(gameObject);
    }

    bool canStayActive()
    {
        if (GameMenu.shifting)
        {
            if (GameMenu.subMenu == GameMenu.SubMenu.Title)
                return GameMenu.shiftingFrom == GameMenu.SubMenu.Credits;
            else if (GameMenu.subMenu == GameMenu.SubMenu.Credits)
                return GameMenu.shiftingFrom == GameMenu.SubMenu.Title;
            else
                return false;
        }
        else
            return GameMenu.subMenu == GameMenu.SubMenu.Title || GameMenu.subMenu == GameMenu.SubMenu.Credits;
    }

    public void setIgnoreWalls(bool ignore)
    {
        if (wallHitCollider == null)
            return;

        foreach (BoxCollider2D wall in spawner.wallColliders)
        {
            Physics2D.IgnoreCollision(wallHitCollider, wall, ignore);
        }
    }
}
