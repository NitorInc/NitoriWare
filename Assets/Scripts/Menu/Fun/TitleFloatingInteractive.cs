using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFloatingInteractive : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private float escapeSpeed;
    [SerializeField]
    private Rigidbody2D _rigidBody;
#pragma warning restore 0649

    void Start()
	{

	}
	
	void LateUpdate()
    {
        if (GameMenu.shifting)
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            Vector2 escapeVelocity = MathHelper.getVector2FromAngle(
                ((Vector2)(transform.position - Camera.main.transform.position)).getAngle(), escapeSpeed);
            transform.position += (Vector3)escapeVelocity * Time.deltaTime;
            if (CameraHelper.isObjectOffscreen(transform, escapeSpeed / 10f))
                Destroy(gameObject);
            return;
        }
    }
}
