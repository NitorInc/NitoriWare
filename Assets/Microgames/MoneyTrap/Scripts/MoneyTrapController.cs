using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTrapController : MonoBehaviour {

    //to check if there are still people moving around
    [Header("Reference to People")]
    [SerializeField]
    private GameObject people;

    // Use this for initialization
    void Start () {

    }
	
	void LateUpdate()
	{
        //player mouse control
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		cursorPosition.y = transform.position.y;
		cursorPosition.z = transform.position.z;
		transform.position = cursorPosition;
	}

    // Update is called once per frame
    void Update() {

        //check if all people were trapped (at least falling)
        if (people.transform.childCount == 0)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
	}
}
