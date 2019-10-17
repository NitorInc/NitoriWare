using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueRescuerController : MonoBehaviour {

    public RumiaRescueRumiaController rumiaController;
    public GameObject rumiaBlackBall;

    [SerializeField]
    private float rescueDistance;

    [SerializeField]
    private GameObject needHelpGroupGO;

    private RumiaRescueStateController[] needHelpList;
    private Transform thisTransform;
    
    void Start() {
        needHelpList = needHelpGroupGO.GetComponentsInChildren<RumiaRescueStateController>();
        thisTransform = transform;
    }

    void Update() {
        float blackBallRange = rescueDistance * 2;
        rumiaBlackBall.transform.localScale = Vector3.one * blackBallRange;

        Vector3 thisPosition = thisTransform.position;
        float realRescueDistance = rescueDistance - 1.5f;
        bool isAllRescued = true; // default true

        for (int i = 0; i < needHelpList.Length; i++) {
            bool isThisRescued = needHelpList[i].CanRescueThisOne(thisPosition, realRescueDistance);
            print(isThisRescued);
            if (isAllRescued == true && isThisRescued == false)
                isAllRescued = false;
        }

        if(isAllRescued) {
            rumiaController.WhenGameVictory();
            MicrogameController.instance.setVictory(true);
        }
    }

    public void ShutDownBlackBall() {
        rescueDistance = 0f;
    }
}
