using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutKnifeStretch : MonoBehaviour {

    public GameObject knifeParent;
    public GameObject knifeRig;

    //Offset used to position the blade properly
    [Header("Minimum X position of knife blade:")]
    public float minPos = -0.124f;
    [Header("Maximum X position of knife blade:")]
    public float maxPos = 0.027f;

    //The scale of the blade. 
    [Header("Minimum X scale of knife blade:")] 
    public float minScale = -0.6550847f;
    [Header("Maximum X scale of knife blade:")]
    public float maxScale = 0.6550847f;
    
    static float t = 0.0f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //Updates the new position of the blade
        transform.position = new Vector2(Mathf.Lerp(knifeRig.transform.position.x + maxPos, knifeRig.transform.position.x + minPos, t), transform.position.y);

        //Updates the new scale of the blade
        transform.localScale = new Vector2(Mathf.Lerp(maxScale, minScale, t), transform.localScale.y);

        /* Sets t equal to the position of the knife
         * Here, the range of the knife's position is used to get a value between 0 and 1
         * TODO: Get the range of how far the knife can move instead of using a constant
        */
        t = ((knifeParent.transform.position.x + 3.5f) * 1f / (3.5f + 3.5f));

	}
}
