using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EikiJudge_SoulController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer soulRenderer;
    [SerializeField]
    private Sprite badSoulSprite;
    [SerializeField]
    private Sprite goodSoulSprite;
    [SerializeField]
    private float delay = 0.25f;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float toPortalSpeed = 25f;
    [SerializeField]
    private float readyDelay = .35f;
    [SerializeField]
    private Vector3 restSpot;
    [SerializeField]
    private AudioClip inClip;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip lossClip;

    public int soulListPosition;
    private bool moveToCourt = false;
    
    public bool Ready => readyDelay <= 0f;

    private Vector3 soulTarget;

    private Vector3 targetPortal = Vector3.zero;
    private Vector3 soulTrajectory;
    private bool soulIsLate = false;
    private bool isLastSoul = false;
    private bool incorrectChoice = false;


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

        // If this soul must be late
        if (EikiJudge_Controller.controller.lastSoulIsLate && (soulListPosition + 1) == EikiJudge_Controller.controller.soulsNumber)
        {
            soulIsLate = true;
        }

        // Put some delay before moving in front of Eiki
        // delay is multiplied by index, so the first delay is null (multiplied by index 0)
        Invoke("MoveToCourt", delay * soulListPosition);

    }


    void Update()
    {
        soulListPosition = EikiJudge_Controller.controller.soulsList.IndexOf(this);
        
        // If he's still on the list, got the order to move and isnt flagged as late
        if (soulListPosition >= 0 && moveToCourt && soulIsLate == false)
        {
            // Move the soul in the correct spot
            soulTarget = restSpot + new Vector3(0f, -1.5f * soulListPosition, soulListPosition * -1f);
            transform.position = Vector3.Lerp(transform.position, soulTarget, Time.deltaTime * speed);
        }
        else if (soulListPosition < 0)
        {
            // Move the soul towards the chosen portal
            Vector3 newPosition = transform.position + (soulTrajectory * toPortalSpeed * Time.deltaTime);
            transform.position = newPosition;
        }

        // If soul Y is higher than the doors Y, then he's past them and should disappear
        if (this.transform.position.y > EikiJudge_PortalsController.controller.transform.position.y)
        {
            var portalChildren = (new int[] { 0, 1 }).Select(a => EikiJudge_PortalsController.controller.transform.GetChild(a));
            var chosenPortal = portalChildren.FirstOrDefault(a => Mathf.Sign(a.position.x) == Mathf.Sign(targetPortal.x));
            chosenPortal.GetComponentInChildren<Animator>().SetTrigger(incorrectChoice ? "Incorrect" : "Correct");

            if (incorrectChoice)
                MicrogameController.instance.playSFX(lossClip, AudioHelper.getAudioPan(transform.position.x));
            else
            {
                MicrogameController.instance.playSFX(inClip, AudioHelper.getAudioPan(transform.position.x));
                if (isLastSoul)
                    MicrogameController.instance.playSFX(winClip);
            }

            this.gameObject.SetActive(false);
            // TODO: Add an effect, particles, ripple on the door ?
            // Maybe fade instead of just deactivate the gameobject ?
        }

        // Check if soul should be late
        if (soulIsLate)
        {
            if (MicrogameController.instance.session.BeatsRemaining <= 3f || soulListPosition == 0)
            {
                soulIsLate = false;
            }
        }
        else
            readyDelay -= Time.deltaTime;

    }

    // Delayed move
    private void MoveToCourt()
    {
        moveToCourt = true;
    }

    // Send the soul to a portal
    public void SendTheSoul(EikiJudge_Controller.Direction judgementDirection, bool isLastSoul)
    {
        this.isLastSoul = isLastSoul;
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
            incorrectChoice = true;
        }
    }
}
