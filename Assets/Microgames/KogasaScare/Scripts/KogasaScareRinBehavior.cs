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
    [SerializeField]
    private KogasaScareKogasaBehaviour kogasa;

    private float startX;
    private bool walkingLeft = true;
    
	void onSpawnSet()
    {
        startX = transform.position.x;
        rigAnimator.SetInteger("direction", walkingLeft ? -1 : 1);
        if ((kogasa.transform.position.x > transform.position.x) != walkingLeft)
            aboutFace(preservePosition:true);
    }
	
	void Update ()
    {
        transform.Translate((walkingLeft ? Vector3.left : Vector3.right) * walkSpeed * Time.deltaTime);
        float walkX = transform.position.x - startX;

        if ((walkingLeft && walkX < -walkRadius)
            || (!walkingLeft && walkX > walkRadius))
            aboutFace();
    }

    void aboutFace(bool preservePosition = false)
    {
        walkingLeft = !walkingLeft;
        rigAnimator.SetInteger("direction", walkingLeft ? -1 : 1);
        if (!preservePosition)
        {
            float xPosition = walkingLeft ? startX + walkRadius : startX - walkRadius;
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
        }
    }

    void onScare(bool victory)
    {
        enabled = false;
    }
}
