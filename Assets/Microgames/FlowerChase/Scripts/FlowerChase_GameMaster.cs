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
    public int Difficulty; // 0 = easy; 1 = normal; 2 = hard
    public GameObject[] DifficultyLevels; // each difficulty is near identical, but each one the "Difficulty" variable is set to a different number, which then instantiates a different difficulty level.
    public float Timer = 5f;
    public GameObject VictoryScreen;
    public Sprite CirnoHit;
    public Sprite YuukaEvilFace;
    public SpriteRenderer CirnoRenderer;
    public SpriteRenderer YuukaRenderer;
    public Animator CirnoAnimator;
    public Animator YuukaAnimator;
    public GameObject SmackObject;
    // Use this for initialization
    void Start() {
        switch (Difficulty)
        {
            case 0:
                
                break;
            case 1:

                break;
            case 2:

                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        switch (Status)
        {

            case 0: // Playing
                if (Timer > 1.2f)
                {
                    Timer -= Time.deltaTime;
                    if (Timer > 1.4f)
                    {

                        
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
                        CameraTrans.position = new Vector3(0f, CameraTrans.position.y, CameraTrans.position.z);
                        CirnoTrans.position = new Vector3(CirnoTrans.position.x, CirnoTrans.position.y, CirnoTrans.position.z - ScrollSpeed * Time.deltaTime);


                    }
                    else
                    {
                        CirnoTrans.position = new Vector3(Mathf.Lerp(CirnoTrans.position.x, 0, Time.deltaTime*2f), CirnoTrans.position.y, CirnoTrans.position.z - ScrollSpeed * (Time.deltaTime * 1.8f));
                        CameraTrans.parent = null;
                    }
 
                }
                else 
                {
                    Status = 2;

                }
                break;

            case 1: // Fail State
                CirnoAnimator.enabled = false;
                YuukaAnimator.enabled = false;
                SmackObject.SetActive(true);
                CirnoRenderer.sprite = CirnoHit;
                YuukaRenderer.sprite = YuukaEvilFace;
                YuukaTrans.position = new Vector3(Mathf.Lerp(YuukaTrans.position.x, CirnoTrans.position.x, Time.deltaTime * YuukaSpeed), YuukaTrans.position.y, YuukaTrans.position.z);
                break;
            case 2:
                VictoryScreen.SetActive(true);
                MicrogameController.instance.setVictory(victory: true, final: true);
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
