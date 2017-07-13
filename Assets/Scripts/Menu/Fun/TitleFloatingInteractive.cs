using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFloatingInteractive : MonoBehaviour
{
    public TitleInteractableSpawner spawner;

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private float startSpeed, lifetime, escapeSpeed;
    [SerializeField]
    Vector2 floatTowardsBounds;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private Collider2D wallHitCollider;
#pragma warning restore 0649

    private bool ignoreWalls;

    void Start()
	{
        setIgnoreWalls(true);

        Vector2 goal = new Vector2(Random.Range(-floatTowardsBounds.x, floatTowardsBounds.x),
            Random.Range(-floatTowardsBounds.y, floatTowardsBounds.y));
        _rigidBody.velocity = (goal - (Vector2)transform.localPosition).resize(startSpeed);
	}
	
	void LateUpdate()
    {
        if (GameMenu.shifting)
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            Vector2 escapeVelocity = MathHelper.getVector2FromAngle(
                ((Vector2)(transform.position - Camera.main.transform.position)).getAngle(), escapeSpeed);
            transform.position += (Vector3)escapeVelocity * Time.deltaTime;
            return;
        }
        else if (lifetime > 0f && ignoreWalls && !CameraHelper.isObjectOffscreen(transform,
            -Mathf.Max(wallHitCollider.bounds.extents.x, wallHitCollider.bounds.extents.y)))
        {
            setIgnoreWalls(false);
        }

        if (lifetime > 0f)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
                setIgnoreWalls(true);
        }

        if (CameraHelper.isObjectOffscreen(transform, 10f))
            Destroy(gameObject);
    }

    public void setIgnoreWalls(bool ignore)
    {
        if (wallHitCollider == null)
            return;

        foreach (BoxCollider2D wall in spawner.wallColliders)
        {
            Physics2D.IgnoreCollision(wallHitCollider, wall, ignore);
        }
        ignoreWalls = ignore;
    }
}
