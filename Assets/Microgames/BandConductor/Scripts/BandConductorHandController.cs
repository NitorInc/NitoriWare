using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BandConductorHandController : MonoBehaviour
{
    [SerializeField]
    private int gesturesRequired;
    [SerializeField]
    private float progressRequired;
    [SerializeField]
    private int notesRequired;
    [SerializeField]
    private Animator[] performerAnimators;
    [SerializeField]
    private Animator conductorAnimator;

    [SerializeField]
    private float animationProgressMult = 2f;
    [SerializeField]
    private float speedMax = 2f;
    [SerializeField]
    private Vector2 speedBounds;
    [SerializeField]
    private float resetSpeed = 15f;
    [SerializeField]
    private float victoryResetSpeed = 30f;
    [SerializeField]
    private float resetTime = .2f;

    [SerializeField]
    private float performerAnimatorIncreaseSpeed = 8f;
    [SerializeField]
    private float performerAnimatorDecreaseSpeed = 5f;
    [SerializeField]
    private float victoryAnimatorSlowSpeed = 2f;

    private float resetCounter;
    private int notesRemaining;

    private float lastProgress;
    private List<BandConductorHandGesture> gestures;
    private BandConductorHandGesture currentGesture;

    void Start ()
    {
        gestures = GetComponents<BandConductorHandGesture>().ToList();
        gestures.Shuffle();
        while (gestures.Count() < gesturesRequired && gestures.Any())
        {
            var newGestures = new List<BandConductorHandGesture>(gestures);
            do
            {
                newGestures.Shuffle();
            }
            while (newGestures.FirstOrDefault() == gestures.Last() || newGestures.Count() <= 1);
            gestures.AddRange(newGestures);
        }

        nextGesture();
        currentGesture.enabled = true;
    }

    void nextGesture()
    {
        if (currentGesture != null)
        {
            currentGesture.enabled = false;
        }
        gesturesRequired--;
        if (gesturesRequired < 0)
        {
            Victory();
            return;
        }
        currentGesture = gestures.FirstOrDefault();
        gestures.RemoveAt(0);
        currentGesture.ResetGesture();
        lastProgress = currentGesture.Progress;
        notesRemaining = notesRequired;
        conductorAnimator.SetTrigger("UIAppear");
        conductorAnimator.SetInteger("UIIndex", currentGesture.GestureIndex);
    }

    void Victory()
    {
        MicrogameController.instance.setVictory(true);
        conductorAnimator.SetTrigger("Victory");
        //foreach (var animator in performerAnimators)
        //{
        //    animator.SetFloat("Speed", 0f);
        //}
    }

    void Update ()
    {
        if (gesturesRequired < 0)
        {
            transform.localPosition = Vector3.zero;
            foreach (var animator in performerAnimators)
            {
                animator.SetFloat("Speed",
                    Mathf.MoveTowards(animator.GetFloat("Speed"), 0f, Time.deltaTime * victoryAnimatorSlowSpeed));
            }
            //transform.moveTowardsLocal2D(Vector2.zero, victoryResetSpeed);

            return;
        }

        var speed = (currentGesture.Progress - lastProgress) / Time.deltaTime;
        foreach (var animator in performerAnimators)
        {
            var goalSpeed = Mathf.Abs(Mathf.Lerp(speedBounds.x, speedBounds.y, speed / speedMax));
            var acc = goalSpeed > animator.GetFloat("Speed")
                ? performerAnimatorIncreaseSpeed: performerAnimatorDecreaseSpeed;
            animator.SetFloat("Speed",
                Mathf.MoveTowards(animator.GetFloat("Speed"), goalSpeed, acc * Time.deltaTime));
        }

        if (resetCounter > 0f)
        {
            transform.moveTowardsLocal2D(currentGesture.getStartPosition(), resetSpeed);
            conductorAnimator.SetFloat("Progress",
                Mathf.MoveTowards(conductorAnimator.GetFloat("Progress"), currentGesture.GetConductorAnimationPosition(), resetSpeed * Time.deltaTime));

            resetCounter -= Time.deltaTime;
            if (resetCounter <= 0f)
            {
                currentGesture.enabled = true;
            }
            else
                return;
        }

        conductorAnimator.SetFloat("Progress", currentGesture.GetConductorAnimationPosition() * .995f);

        lastProgress = currentGesture.Progress;

        if (currentGesture.Progress >= progressRequired)
        {
            nextGesture();
            resetCounter = resetTime;
        }
	}

    public void RegisterNote()
    {
        notesRemaining--;
        if (notesRemaining <= 0)
        {
            nextGesture();
            resetCounter = resetTime;
        }
    }
}
