using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareRinBone : MonoBehaviour
{
    [SerializeField]
    private Vector2 xSpeedRange;
    [SerializeField]
    private Vector2 ySpeedRange;
    [SerializeField]
    private Vector2 rotateSpeedRange;
    [SerializeField]
    private float destroyY;
    [SerializeField]
    private Animator rinAnimator;

    private Rigidbody2D rigidBoi;
    
	
	public void Start()
    {
        rigidBoi = GetComponent<Rigidbody2D>();

        float direction = (float)rinAnimator.GetInteger("direction");
        if (direction == 0f)
            direction = -1f;

        rigidBoi.velocity = new Vector2(
            MathHelper.randomRangeFromVector(xSpeedRange) * direction
            , MathHelper.randomRangeFromVector(ySpeedRange));
        rigidBoi.angularVelocity = MathHelper.randomRangeFromVector(rotateSpeedRange) * direction;
    }

    private void Update()
    {
        if (transform.position.y < destroyY)
            Destroy(gameObject);
    }
}
