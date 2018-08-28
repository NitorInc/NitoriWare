using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EikiJudge_SoulController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer soulRenderer;
    [SerializeField]
    private Sprite badSoulSprite, goodSoulSprite;

    public int soulListPosition;
    private EikiJudge_Controller.SoulType soulType;

    private bool moveToCourt = false;
    private float delay = 0.25f;

    private Vector3 soulTarget;
    private float speed = 10f;

    private Vector3 targetPortal = Vector3.zero;
    private Vector3 soulTrajectory;

    public EikiJudge_Controller.Direction RightPortalDirection { get; set; }


    private void Start()
    {
        // Randomize bad/good soul
        if (Random.value > 0.5f)
        {
            soulRenderer.sprite = goodSoulSprite;
            soulType = EikiJudge_Controller.SoulType.good;
            RightPortalDirection = EikiJudge_PortalsController.portalsController.heavenDirection;
        }
        else
        {
            soulRenderer.sprite = badSoulSprite;
            soulType = EikiJudge_Controller.SoulType.bad;
            RightPortalDirection = EikiJudge_PortalsController.portalsController.hellDirection;
        }

        // Put some delay before moving in front of Eiki
        Invoke("MoveToCourtDelay", delay * soulListPosition);
    }


    void Update()
    {
        // If he's still on the list / waiting line
        soulListPosition = EikiJudge_Controller.controller.soulsList.IndexOf(this);

        if (soulListPosition >= 0 && moveToCourt)
        {
            // Move the soul in the correct spot
            soulTarget = new Vector3(0f, -1.5f * soulListPosition, soulListPosition * -1f);
            transform.position = Vector3.Lerp(transform.position, soulTarget, Time.deltaTime * speed);
        }
        else
        {
            // Move the soul towards the chosen portal
            Vector3 newPosition = transform.position + (soulTrajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }

    }

    // Delayed move
    private void MoveToCourtDelay()
    {
        moveToCourt = true;
    }

    // Send the soul to a portal
    public void SendTheSoul(EikiJudge_Controller.Direction judgementDirection)
    {
        if (judgementDirection == EikiJudge_Controller.Direction.left)
        {
            targetPortal = new Vector3(-4.35f, 2.5f);
        }
        else
        {
            targetPortal = new Vector3(4.35f, 2.5f);
        }

        soulTrajectory = (targetPortal - transform.position).normalized;

        // If the soul is not sent to the right portal -> loose
        if (judgementDirection != RightPortalDirection)
        {
            EikiJudge_Controller.controller.LoseGame();
        }
    }
}
