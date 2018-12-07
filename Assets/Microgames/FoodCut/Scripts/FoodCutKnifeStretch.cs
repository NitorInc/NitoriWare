using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutKnifeStretch : MonoBehaviour {

    public GameObject knifeParent;

    //The scale of the blade. 
    [Header("Minimum X scale of knife blade:")] 
    public float minScale = -0.6550847f;
    [Header("Maximum X scale of knife blade:")]
    public float maxScale = 0.6550847f;

    [Header("Minimum X position of movement")]
    public float minX = 0f;
    [Header("Maximum X position of movement")]
    public float maxX = 0f;
    
    static float t = 0.0f;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        //Updates the new scale of the blade
        transform.localScale = new Vector2(Mathf.Lerp(maxScale, minScale, t), transform.localScale.y);

        /* Sets t equal to the position of the knife
         * Here, the range of the knife's position is used to get a value between 0 and 1
         * TODO: Get the range of how far the knife can move instead of using a constant
        */
        t = ((knifeParent.transform.position.x - minX) * 1f / (maxX - minX));

	}
}
