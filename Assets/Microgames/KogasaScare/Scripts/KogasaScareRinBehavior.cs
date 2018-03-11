using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareRinBehavior : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float walkRadius;
    [SerializeField]
    private Animator rigAnimator;

    private float startX;
    private bool walkingLeft = true;
    
	void onSpawnSet()
    {
        startX = transform.position.x;
        rigAnimator.SetInteger("direction", walkingLeft ? -1 : 1);
    }
	
	void Update ()
    {
        transform.Translate((walkingLeft ? Vector3.left : Vector3.right) * walkSpeed * Time.deltaTime);
        float walkX = transform.position.x - startX;

        if (walkingLeft && walkX < -walkRadius)
        {
            transform.position = new Vector3(startX - walkRadius, transform.position.y, transform.position.z);
            walkingLeft = false;
            rigAnimator.SetInteger("direction", 1);
        }
        else if (!walkingLeft && walkX > walkRadius)
        {
            transform.position = new Vector3(startX + walkRadius, transform.position.y, transform.position.z);
            walkingLeft = true;
            rigAnimator.SetInteger("direction", -1);
        }
    }

    void onScare(bool victory)
    {
        enabled = false;
    }
}
