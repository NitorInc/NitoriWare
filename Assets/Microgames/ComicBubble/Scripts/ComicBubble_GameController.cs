using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ComicBubble_GameController : MonoBehaviour {


    [SerializeField]
    List<ComicBubble_ComicData> comicDataList;

    [SerializeField]
    Color deactivatedStripColor;

    [SerializeField]
    Color bubbleShadowColor;

    int currentIndex;

    GameObject currentBubbleShadow;

    ComicBubble_SpeechBubble currentBubbleScript;

    // Use this for initialization
    void Start() {

        // Initialize the bubble gameobjects (stablish the relationship between them, the target and speech speed)
        comicDataList.initializeBubbleGameObjects();

        // Disables all of bubbles so they don't appear all of them at once
        comicDataList.disableAllSpeechBubbles();

        // Deactivate every panel animator
        stopEveryStripAnimator();

        // Hide every panel at first so they don't appear all of them at once
        comicDataList.hideAllStrips(deactivatedStripColor);

        hideAllIndicators();

        // Initalize the bubble index in 0
        currentIndex = 0;

        // Show the elements asociated with that index
        showCurrentElements();


    }


    // Update is called once per frame
    void Update() {


    }



    // Event for changing to the next bubble
    public void eventBubbleHasEnded()
    {
        // Unfollow the current Bubble
        unfollowCurrentBubble();

        // Hide the current Shadow
        hideCurrentBubbleShadow();

        // Hide the current Indicator
        hideCurrentIndicator();

        // Stop the Animators (Strip and Bubble)
        stopStripAnimator(currentIndex);
        stopCurrentBubbleAnimator();

        // Move Current Bubble Upwards
        StartCoroutine(moveBubbleToFinalPosition(currentIndex));

        // Update and show next elements
        currentIndex++;

        updateAnimatorIndexParameter();

        if (currentIndex < comicDataList.Count)
        {
            showCurrentElements();

            // If the next step doesn't have a speechbubble attached, then it's assumes that the game has finished...
            if (getCurrentBubble() == null)
            {
                microgameEnd();
            }
        }

        // No more bubbles or panels
        else
        {
            microgameEnd();
        }
    }


   


    //  This is called after one of the bubbles ends and the current index is updated. It shows everything related to that index. 
    void showCurrentElements()
    {

        showCurrentStrip();

        resumeStripAnimator(currentIndex);

        showCurrentBubble();

        showCurrentIndicator();

        showCurrentBubbleShadow();

        updateCurrentBubbleTextScript();

  
    }


    //  To finally end the game
    public void microgameEnd()
    {
        MicrogameController.instance.setVictory(true);
    }


    //  ANIMATOR STUFF


    //  To update the animator index parameter
    void updateAnimatorIndexParameter()
    {

        for (int i = 0; i < comicDataList.Count; i++)
        {
            if (comicDataList.usesAnimatorParameter(i))
            {
                var animator = comicDataList.getStrip(i).GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetInteger("CurrentIndex", currentIndex);
                }
            }
        }
    }



    void stopEveryStripAnimator()
    {
        foreach(var element in comicDataList)
        {
            var strip = element.getStrip();
            var animator = strip.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
        }
    }

    void stopStripAnimator(int index)
    {
        if (comicDataList.stripAnimatorCanBeDisabled(index))
        {
            var animator = comicDataList.getStrip(index).GetComponent<Animator>();

            if (animator != null)
            {
                animator.enabled = false;
            }

            else
            {
                print("Want to disable animator but there is no animator...");
            }
        }
    }

    void resumeStripAnimator(int index)
    {
        var animator = comicDataList.getStrip(index).GetComponent<Animator>();
        if (animator != null)
            animator.enabled = true;

    }


    void stopCurrentBubbleAnimator()
    {
        comicDataList.disableAnimator(currentIndex);
    }
    

//  CURRENT STRIP RELATED STUFF


    //  To get the current strip displayed.
    GameObject getCurrentStrip()
    {
        if (currentIndex < comicDataList.Count)
        {
            return comicDataList[currentIndex].getStrip();
        }

        else
        {
            return null;
        }

    }


    //  For showing the strip related to the current index
    void showCurrentStrip()
    {
        //  Previous strip is sent to the back 
        if (currentIndex > 0)
        {
            // But only if it's different from tu current strip
            var previousStrip = comicDataList.getStrip(currentIndex - 1);
            var actualStrip = comicDataList.getStrip(currentIndex);
            if (previousStrip != actualStrip)
            {
                comicDataList.sendStripToTheBack(currentIndex - 1);
            }
        }


        comicDataList.showStrip(currentIndex);
    }


//  INDICATOR RELATED STUFF
    
    void hideAllIndicators()
    {
        foreach( ComicBubble_ComicData data in comicDataList)
        {
            var indicator = data.getIndicator();
            if (indicator != null)
            {
                indicator.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }

        }
    }

    void showCurrentIndicator()
    {
        var indicator = comicDataList.getIndicator(currentIndex);
        if (indicator != null)
        {
            indicator.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }

    }

    void hideCurrentIndicator()
    {
        var indicator = comicDataList.getIndicator(currentIndex);
        if (indicator != null)
        {
            GameObject.Destroy(indicator);
        }

    }

    //  CURRENT BUBBLE RELATED STUFF


    //  Function to get the current bubble displayed.
    GameObject getCurrentBubble()
    {
        return comicDataList[currentIndex].getBubble();
    }


    // Enables (shows) the current bubble object
    void showCurrentBubble()
    {
        comicDataList.enableSpeechBubble(currentIndex);
    }


    // Disables FollowCursor of the current bubble object
    void unfollowCurrentBubble()
    {
        comicDataList.unfollowSpeechBubble(currentIndex);
    }


    //  Updates the speechbubble script for easy access
    void updateCurrentBubbleTextScript()
    {
        currentBubbleScript = comicDataList.getSpeechBubbleScript(currentIndex);
    }


    // Move the bubble, reference by the index, slightly upwards
    IEnumerator moveBubbleToFinalPosition(int index)
    {

        Vector2 originalPosition = (Vector2)comicDataList.getBubble(index).transform.position;

        Vector2 targetPosition = originalPosition + new Vector2(0, comicDataList.getDistanceAfterCompletion(index));

        Transform bubbleTransform = comicDataList[index].getBubble().transform;

        bubbleTransform.GetComponentInChildren<ComicBubble_SpeechBubble>().onFinishedTalking();

        float step = comicDataList.getSpeedAfterCompletion(index) * Time.deltaTime;

        while (!Mathf.Approximately(((Vector2)bubbleTransform.position - targetPosition).sqrMagnitude, 0))
        {
            bubbleTransform.position = Vector2.MoveTowards(bubbleTransform.position, targetPosition, step);
            yield return null;
        }
    }


//  CURRENT BUBBLE SHADOW RELATED STUFF


    //  Instantiate (shows) a shadow from the current bubble
    void showCurrentBubbleShadow()
    {
        // Just one bubble shadow at the time
        hideCurrentBubbleShadow();

        // Get the speechbubble image
        GameObject speechBubble = getCurrentBubble();

        if (speechBubble != null)
        {
 
            GameObject bubbleImage = speechBubble.GetComponentInChildren<SpriteRenderer>().gameObject;

            // Instatiating shadow
            currentBubbleShadow = Instantiate(speechBubble, comicDataList[currentIndex].getStrip().GetComponentInChildren<Canvas>().transform) as GameObject;
            currentBubbleShadow.transform.position = Vector2.zero;
            currentBubbleShadow.transform.SetSiblingIndex(0);

            // Delete non needed commponents
            Destroy(currentBubbleShadow.GetComponentInChildren<ComicBubble_SpeechBubble>());
            Destroy(currentBubbleShadow.GetComponentInChildren<Text>());


            // Add the remaining elements
            foreach(SpriteRenderer sprite in currentBubbleShadow.GetComponentsInChildren<SpriteRenderer>())
            {
                sprite.color = bubbleShadowColor;
                sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                sprite.sortingOrder = 1;
            }
;
        }

    }


    //  Destroys the shadow from the current bubble
    void hideCurrentBubbleShadow()
    {
        // Destroy the current bubble shadow
        if (currentBubbleShadow != null)
        {
            GameObject.Destroy(currentBubbleShadow);
            currentBubbleShadow = null;
        }
    }



}

