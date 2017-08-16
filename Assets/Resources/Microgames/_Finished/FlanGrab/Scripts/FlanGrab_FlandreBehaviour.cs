using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_FlandreBehaviour : MonoBehaviour {

    private GameObject rightArmObject;
    //private float xRightLimit;

	[SerializeField]
	private float bodyRotationMult;


    // Use this for initialization
    void Start () {
        //var microgrameScript = MicrogameController.instance.GetComponent<FlanGrab_Microgame_Behaviour>();
        //xRightLimit = microgrameScript.rightLimit;

        rightArmObject = this.transform.FindChild("Right_Arm").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        if (!MicrogameController.instance.getVictoryDetermined())
        {
            rotateRightArm();
        }
    }

    void rotateRightArm()
    {
        var mouseOnScreen = CameraHelper.getCursorPosition();
        var positionOnScreen = rightArmObject.transform.position;
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        if (Mathf.Abs(angle) <= 90f)
        {
            rightArmObject.transform.rotation = Quaternion.Euler(Vector3.forward * angle));
			transform.rotation = Quaternion.Euler(Vector3.forward * angle * bodyRotationMult));
        }
    }
    

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
