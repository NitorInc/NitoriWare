using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueRescuerController : MonoBehaviour {

    public RumiaRescueRumiaController rumiaController;
    public GameObject rumiaBlackBall;

    [SerializeField]
    private float rescueDistance;

    [SerializeField]
    private string needHelpGroupName = "NeedHelpGroup";

    private Transform needHelpGroupTF;
    private RumiaRescueStateController[] needHelpList;
    private Transform thisTransform;
    
    void Start() {
        needHelpGroupTF = transform.parent.Find(needHelpGroupName);
        needHelpList = needHelpGroupTF.GetComponentsInChildren<RumiaRescueStateController>();
        thisTransform = transform;
    }

    void Update() {
        if (rumiaController.IsFinished == true)
            return;
        if (rescueDistance <= 0f)
            return;

        float blackBallRange = rescueDistance * 2;
        rumiaBlackBall.transform.localScale = Vector3.one * blackBallRange;

        Vector3 thisPosition = thisTransform.position;
        float realRescueDistance = rescueDistance - 1.5f;
        bool isAllRescued = true; // default true

        for (int i = 0; i < needHelpList.Length; i++) {
            bool isThisRescued = needHelpList[i].CanRescueThisOne(thisPosition, realRescueDistance);
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

        for (int i = 0; i < needHelpList.Length; i++) {
            needHelpList[i].MakeThisOneFeelHot();
        }

    }
}
