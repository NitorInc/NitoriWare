using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutKnifeAnim : MonoBehaviour {
    /* Placeholder knife animation script until
       I can figure out how to put evertying in
       the triggeranim script
    */
    Animator foodCutAnim;

    // Use this for initialization
    void Start () {
        foodCutAnim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (MicrogameController.instance.getVictory() == true)
        {
            foodCutAnim.SetTrigger("Victory");
        }
    }
}
