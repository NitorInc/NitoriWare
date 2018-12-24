using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaSpinArrows : MonoBehaviour {

    private float currentScale;
    private Vector3 originalScaleValues;
    private Vector3 zeroVect;
    private int rotSpeed;
    public bool flipped;

	// Use this for initialization
	void Start () {

        rotSpeed = 1;
        zeroVect = new Vector3(0,0,0);

        flipped = MathHelper.randomBool();
        if (flipped)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        originalScaleValues = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

        if (!MicrogameController.instance.getVictory() && !Input.GetMouseButton(0))
        {
            /*
            if(transform.localScale == zeroVect)
            {
                if(flipped)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                } else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                
            }
            currentScale = (float)(1.1f + Mathf.PingPong(Time.time * 0.5f, 0.2f));
            transform.localScale = new Vector3(originalScaleValues.x * currentScale, originalScaleValues.y * currentScale, originalScaleValues.z);
            */
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 1 * rotSpeed * System.Convert.ToInt32(flipped));
            transform.localScale = new Vector3(originalScaleValues.x, originalScaleValues.y, originalScaleValues.z);
        } else
        {
            transform.localScale = zeroVect;
        }
	}
}
