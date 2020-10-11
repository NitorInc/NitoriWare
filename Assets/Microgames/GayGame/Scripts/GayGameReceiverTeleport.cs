using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameReceiverTeleport : MonoBehaviour {

    [SerializeField]
    private Vector2 xRange;
    [SerializeField]
    private float minDistanceFromSender = 2f;
    [SerializeField]
    private float minDistanceFromLastPos = 1.5f;
    [SerializeField]
    private Transform handTransform;
    [SerializeField]
    private Transform letterTransform;
    [SerializeField]
    private Transform senderTransform;
    [SerializeField]
    private GayGameSenderGrabLetter letterGrabber;

	void Start ()
    {
        teleport();
	}
	
    public void teleport()
    {
        var playerTransform = letterGrabber.Grabbed ? senderTransform : letterTransform;
        int tries = 100;
        var lastPos = handTransform.position.x;
        do
        {
            handTransform.position = new Vector3(MathHelper.randomRangeFromVector(xRange),
                handTransform.position.y, handTransform.position.z);
            tries--;
        }
        while (tries > 0 &&
        (Mathf.Abs(handTransform.position.x - playerTransform.position.x) < minDistanceFromSender
        || (Mathf.Abs(handTransform.position.x - lastPos) < minDistanceFromSender)));

        if (tries <= 0)
            print("AAA thats too many tries");
    }
}
