using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerChase_GameMaster : MonoBehaviour {
    public Transform CirnoTrans;
    public Transform CameraTrans;
    public Transform YuukaTrans;
    public float CirnoMovementSpeed;
    public float ScrollSpeed;
    public float YuukaSpeed;
    public int Status;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        switch (Status)
        {

            case 0: // Playing
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (CirnoTrans.position.x > -0.38f) CirnoTrans.position = new Vector3(CirnoTrans.position.x - CirnoMovementSpeed * Time.deltaTime, CirnoTrans.position.y, CirnoTrans.position.z);

                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (CirnoTrans.position.x < 0.38f) CirnoTrans.position = new Vector3(CirnoTrans.position.x + CirnoMovementSpeed * Time.deltaTime, CirnoTrans.position.y, CirnoTrans.position.z);
                }


                if (CirnoTrans.position.x < -0.38f) CirnoTrans.position = new Vector3(-0.38f, CirnoTrans.position.y, CirnoTrans.position.z);
                if (CirnoTrans.position.x > 0.38f) CirnoTrans.position = new Vector3(0.38f, CirnoTrans.position.y, CirnoTrans.position.z);


                YuukaTrans.position = new Vector3(Mathf.Lerp(YuukaTrans.position.x, CirnoTrans.position.x, Time.deltaTime * YuukaSpeed), YuukaTrans.position.y, YuukaTrans.position.z - ScrollSpeed * Time.deltaTime);
                CirnoTrans.position = new Vector3(CirnoTrans.position.x, CirnoTrans.position.y, CirnoTrans.position.z - ScrollSpeed * Time.deltaTime);
                CameraTrans.position = new Vector3(0f, CameraTrans.position.y, CameraTrans.position.z);
                break;

            case 1: // Fail State
                YuukaTrans.position = new Vector3(Mathf.Lerp(YuukaTrans.position.x, CirnoTrans.position.x, Time.deltaTime * YuukaSpeed), YuukaTrans.position.y, YuukaTrans.position.z);
                break;
        }
        
    }

    public void HitBlock(){ // or probably a row of flowers
        if (Status == 0)
        {
            MicrogameController.instance.setVictory(victory: false, final: true);

            Status = 1;
        }
        
    }
}
