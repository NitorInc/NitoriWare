using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTrapController : MonoBehaviour {

    //to check if there are still people moving around
    private GameObject people;

    // Use this for initialization
    void Start () {
        people = GameObject.Find("People");
    }
	
	void LateUpdate()
	{
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		cursorPosition.y = transform.position.y;
		cursorPosition.z = transform.position.z;
		transform.position = cursorPosition;
	}

    // Update is called once per frame
    void Update() {
        //how many people alive (+1 for self)
        Transform[] peoplearr = people.GetComponentsInChildren<Transform>();

        if (peoplearr.Length < 2)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }

        print(peoplearr.Length);
	}
}
