using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckVacuum : MonoBehaviour
{
    private float angle;
    [SerializeField]
    private float maxangleleft, minangleleft, maxangleright2, minangleright2, maxangleright, minangleright,leftset, rightset;
    [SerializeField]
    private float angleoffset;
    //factor of angle, does not affect adjustment
    [SerializeField]
    private float anglemultiplicative;
    [SerializeField]
    private GameObject tiltreference;
    [SerializeField]
    private float delay;
    [SerializeField]
    private bool updated;
    
    

    private SpriteRenderer spriteRenderer;


    void Update()
    {
        Vector3 relative = CameraHelper.getCursorPosition();

        angle = ((Vector2)(transform.position - relative)).getAngle();

        setAngle();
        if (updated == false)
        {
            updated = true;
            Invoke("updateRotation", delay);
        }

    }

    private void setAngle()
    {
        
       if (angle >= minangleright && angle <= maxangleright)
        {
            angle = rightset;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - angleoffset);
        }
        else if (angle >= minangleright2 && angle <= maxangleright2)
        {
            angle = rightset;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - angleoffset);
        }
        else if (angle > minangleleft && angle < maxangleleft)
        {
            angle = leftset;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - angleoffset);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle - angleoffset);
        }
    }

    void updateRotation()
    {
        tiltreference.transform.rotation = Quaternion.Euler(0f, 0f, angle - angleoffset);
        if (updated == true)
        updated = false;
    }




}



