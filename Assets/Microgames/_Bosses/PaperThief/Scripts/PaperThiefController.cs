using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefController : MonoBehaviour
{
    public static PaperThiefController instance;

#pragma warning disable 0649
    [SerializeField]
    private Animator sceneAnimator;
    [SerializeField]
    private Transform shootCluster;
    [SerializeField]
    private Scene scene;
#pragma warning restore 0649

    private AnimationEventHelper eventHelper;

    public enum Scene
    {
        Idle,
        CucumberSteal,
        BeginChase,
        BeginFight,
        MarisaDeath,
        Victory
    }

    void Awake()
    {
        instance = this;
        eventHelper = GetComponent<AnimationEventHelper>();
    }

    void Update()
    {
        if (scene == Scene.BeginChase && shootCluster.childCount == 0)
            startScene(Scene.BeginFight);
    }

    public void startScene(Scene scene)
    {
        this.scene = scene;
        sceneAnimator.SetInteger("QueuedAnimation", (int)scene);

        // Hotfix for some users possibly not encountering animation events at start of animation
        switch(scene)
        {
            case (Scene.CucumberSteal):
                eventHelper.triggerEvent(0);
                setMarisaFacingRight(1);
                queueMarisaAnimation(PaperThiefMarisa.QueueAnimation.Zoom);
                eventHelper.fadeAudio(0);
                break;
            case (Scene.BeginChase):
                eventHelper.playAudio(1);
                break;
        }
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

        PaperThiefCamera.instance.setFollow(null);
        PaperThiefCamera.instance.setGoalPosition(PaperThiefNitori.instance.transform.position + new Vector3(5f, 6f, 0f));
        PaperThiefCamera.instance.setGoalSize(6.5f);
    }

    public void detachFromNitori()
    {
        PaperThiefCamera.instance.transform.parent = transform;
    }

    public void displayShootMessage()
    {
        MicrogameController.instance.displayLocalizedCommand("commandc", "Shoot Down!");
    }

    public void startFight()
    {
        sceneAnimator.enabled = false;
        MicrogameController.instance.displayLocalizedCommand("commandd", "Defeat Her!");
        PaperThiefMarisa.instance.ChangeState(PaperThiefMarisa.State.Fight);
    }


    public void endFight()
    {
        sceneAnimator.enabled = true;
        startScene(Scene.MarisaDeath);
    }
}

