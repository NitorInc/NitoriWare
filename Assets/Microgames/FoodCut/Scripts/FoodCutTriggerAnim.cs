using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCutTriggerAnim : MonoBehaviour {
    Animator foodCutAnim;
    [Header("Name of animation that plays")]
    [SerializeField]
    private string anim;

    // Use this for initialization
    void Start () {
        foodCutAnim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (MicrogameController.instance.getVictory() == true)
        {
            foodCutAnim.Play(anim);
        }
	}
}
