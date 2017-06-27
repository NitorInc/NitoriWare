using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefController : MonoBehaviour
{
    public static PaperThiefController instance;

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Animator sceneAnimator;
#pragma warning restore 0649

    public enum Scene
    {
        CucumberSteal       //0
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {

    }

    public void startScene(Scene scene)
    {
        sceneAnimator.SetInteger("QueuedAnimation", (int)scene);
    }
    
    public void queueNitoriAnimation(PaperThiefNitori.QueueAnimation animation)
    {
        PaperThiefNitori.instance.queueAnimation(animation);
    }

    public void queueMarisaAnimation(PaperThiefMarisa.QueueAnimation animation)
    {
        PaperThiefMarisa.instance.queueAnimation(animation);
    }

    public void setNitoriFacingRight(int facingRight)
    {
        PaperThiefNitori.instance.setFacingRight(facingRight > 0);
    }

    public void setMarisaFacingRight(int facingRight)
    {
        PaperThiefMarisa.instance.setFacingRight(facingRight > 0);
    }

    public void zoomOutCamera()
    {
        PaperThiefCamera.instance.followNitori = false;

        //PaperThiefCamera.instance.transform.parent = PaperThiefNitori.instance.transform;
        PaperThiefCamera.instance.setFollow(null);
        PaperThiefCamera.instance.setGoalPosition(PaperThiefNitori.instance.transform.position + new Vector3(5f, 6f, 0f));
        PaperThiefCamera.instance.setGoalSize(6.5f);
    }

    public void detachFromNitori()
    {
        PaperThiefCamera.instance.transform.parent = transform;
    }

}

