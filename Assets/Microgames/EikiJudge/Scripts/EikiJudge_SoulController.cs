using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EikiJudge_SoulController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer soulRenderer;
    [SerializeField]
    private Sprite badSoulSprite;
    [SerializeField]
    private Sprite goodSoulSprite;

    public int soulListPosition;
    [SerializeField]

    private bool moveToCourt = false;
    private float delay = 0.25f;

    private Vector3 soulTarget;
    private float speed = 10f;

    private Vector3 targetPortal = Vector3.zero;
    private Vector3 soulTrajectory;
    private bool soulIsLate = false;


    public EikiJudge_Controller.Direction rightPortalDirection;


    private void Start()
    {
        // Randomize bad/good soul
        if (Random.value > 0.5f)
        {
            soulRenderer.sprite = goodSoulSprite;
            rightPortalDirection = EikiJudge_PortalsController.controller.heavenDirection;
        }
        else
        {
            soulRenderer.sprite = badSoulSprite;
            rightPortalDirection = EikiJudge_PortalsController.controller.hellDirection;
        }
        print("soul " + soulListPosition + " right portal is " + rightPortalDirection);

        // If this soul must be late
        if (EikiJudge_Controller.controller.lastSoulIsLate && (soulListPosition + 1) == EikiJudge_Controller.controller.soulsNumber)
        {
            soulIsLate = true;
        }

        // Put some delay before moving in front of Eiki
        Invoke("MoveToCourtDelay", delay * soulListPosition);

    }


    void Update()
    {
        soulListPosition = EikiJudge_Controller.controller.soulsList.IndexOf(this);
        
        // If he's still on the list, got the order to move and isnt flagged as late
        if (soulListPosition >= 0 && moveToCourt && soulIsLate == false)
        {
            // Move the soul in the correct spot
            soulTarget = new Vector3(0f, -1.5f * soulListPosition, soulListPosition * -1f);
            transform.position = Vector3.Lerp(transform.position, soulTarget, Time.deltaTime * speed);
        }
        else if (soulListPosition < 0)
        {
            // Move the soul towards the chosen portal
            Vector3 newPosition = transform.position + (soulTrajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }

        // If soul Y is higher than the doors Y, then he's past them and should disappear
        if (this.transform.position.y > EikiJudge_PortalsController.controller.transform.position.y)
        {
            this.gameObject.SetActive(false);
            // TODO: Add an effect, particles, ripple on the door ?
            // Maybe fade instead of just deactivate the gameobject ?
        }

        // Check if soul should be late
        if (soulIsLate)
        {
            if (MicrogameTimer.instance.beatsLeft <= 3 || soulListPosition == 0)
            {
                soulIsLate = false;
            }
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
        if (judgementDirection != rightPortalDirection)
        {
            EikiJudge_Controller.controller.LoseGame();
        }
    }
}
