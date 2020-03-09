using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleBehavior : MonoBehaviour {

    [SerializeField]
    private Collider2D colliderToHit;
    [SerializeField]
    private Collider2D collider;
    bool result = false;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse was clicked!");
            result = collider.IsTouching(colliderToHit);
            if(result == true)
            {
                MicrogameController.instance.setVictory(victory: true, final: true);
                print("Win!");
            }
            else if (result == false)
            {
                MicrogameController.instance.setVictory(victory: false, final: true);
                print("Lose!");
            }
        }
	}
}
