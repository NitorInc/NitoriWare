using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ComicBubble_GameController : MonoBehaviour {

    [System.Serializable]
    class ComicData
    {
        public GameObject strip;
        public GameObject speechBubble;         
        public GameObject characterTarget;
        public float speechSpeed;
    }

    [SerializeField]
    List<ComicData> comicDataList;

    [SerializeField]
    Color deactivatedStripColor;

    [SerializeField]
    float bubbleMovementDistanceAfterEnding;

    [SerializeField]
    float bubbleMovementSpeed;

    [SerializeField]
    Color bubbleShadowColor;

    int currentIndex;

    GameObject currentBubbleShadow;

    ComicBubble_SpeechBubble currentBubbleTextScript;

    Animator animator;

    // Use this for initialization
    void Start() {

        // Set the text speed for each bubble
        initializeAllComicBubbles();

        // Disables all of bubbles so they don't appear all of them at once
        disableAllSpeechBubbles();

        // Hide every panel at first so they don't appear all of them at once
        hideAllStrips();

        // Initalize the bubble index in 0
        currentIndex = 0;

        // Get the animator
        animator = GetComponent<Animator>();

        // Show the elements asociated with that index
        showCurrentElements();


    }

    // Update is called once per frame
    void Update() {


    }


    //  This puts the data that the speechbubble script needs in order to function correctly.
    //  This is: The bubble target and the speed in which text will appear
    void initializeAllComicBubbles()
    {
        foreach (ComicData d in comicDataList) {
            if (d.speechBubble != null)
            {
                if (d.characterTarget != null)
                {
                    var bubbleScript = d.speechBubble.GetComponent<ComicBubble_SpeechBubble>();
                    bubbleScript.setTargetCharacter(d.characterTarget);
                    bubbleScript.setTextSpeed(d.speechSpeed);
                }

                else
                {
                    Debug.Log("No speech target attached to the bubble");
                }
            }
        }
    }
    
  
    //  This is called after one of the bubbles ends and the current index is updated. It shows everything related to that index. 
    void showCurrentElements()
    {
        showCurrentStrip();

        showCurrentBubble();

        showCurrentBubbleShadow();

        updateCurrentBubbleTextScript();

        updateAnimatorIndexParameter();

    }


    //  This is called to hide everything related to the current index.
    void hideCurrentElements()
    {
        unfollowCurrentBubble();

        hideCurrentBubbleShadow();
    }


    //  Function to get the current bubble displayed.
    GameObject getCurrentBubble()
    {
        return comicDataList[currentIndex].speechBubble;
    }


    //  Function to get the current strip displayed.
    GameObject getCurrentStrip()
    {
        return comicDataList[currentIndex].strip;

    }


    //  To update the animator index parameter
    void updateAnimatorIndexParameter()
    {
        if (animator != null)
        {
            animator.SetInteger("CurrentIndex", currentIndex);
        }
    }


/**
 *      PANEL STUFF
 **/

    //  To showing the strip related to the current index
    void showCurrentStrip()
    {

        //  Previous strip is sent to the back so the sprites of the current strip doesn't overlap the mask of the previous strip
        sendPreviousStripToTheBack();  

        sendCurrentStripToTheTop();

        foreach (SpriteRenderer sr in getCurrentStrip().GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material.color = Color.white;
        }

    }


    //  To hide all strips (used at the start of the game)
    void hideAllStrips()
    {

        for (int index = 0; index < comicDataList.Count; index++)
        {
            sendStripToTheBack(index);

            var strip = comicDataList[index].strip;

            foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.color = deactivatedStripColor;
            }

        }

    }


    //  Send the current strip to the "top" so the mask doesn't overlaps with others
    void sendCurrentStripToTheTop()
    {
        sendStripToTheFront(currentIndex);
    }


    //  Send the current strip to the "back" so the mask doesn't overlaps with others
    void sendPreviousStripToTheBack()
    {
        if (currentIndex > 0)
            sendStripToTheBack(currentIndex-1);
    }


    //  Changes the order in layer of every sprite, canvas and spritemask range of the strip related to the index, so they don't overlap with the elements of other strip
    void sendStripToTheFront(int index)
    {

        var strip = comicDataList[index].strip;

        var scaling = 100 * (comicDataList.Count - index);

        // Update Spritemask Range
        var sprmask = strip.GetComponentInChildren<SpriteMask>();
        sprmask.frontSortingOrder = sprmask.frontSortingOrder + scaling;
        sprmask.backSortingOrder = sprmask.backSortingOrder + scaling;

        // Update Canvas sorting order
        var canvas = strip.GetComponentInChildren<Canvas>();
        if (canvas != null) canvas.sortingOrder = canvas.sortingOrder + scaling;

        // Update Sprites sorting order
        foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = sr.sortingOrder + scaling;
    }


    //  Changes the order in layer of every sprite, canvas and spritemask range of the strip related to the index, so they don't overlap with the elements of other strip
    void sendStripToTheBack(int index)
    {

        var strip = comicDataList[index].strip;

        var scaling = 100 * (comicDataList.Count - index);

        // Update Spritemask Range
        var sprmask = strip.GetComponentInChildren<SpriteMask>();
        sprmask.backSortingOrder = sprmask.backSortingOrder - scaling;
        sprmask.frontSortingOrder = sprmask.frontSortingOrder - scaling;

        // Update Canvas sorting order
        var canvas = strip.GetComponentInChildren<Canvas>();
        if (canvas != null) canvas.sortingOrder = canvas.sortingOrder - scaling;

        // Update Sprites sorting order
        foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = sr.sortingOrder - scaling;
    }




/**
*  BUBBLE - STUFF
**/


    // Disables all of the speech bubbles contained in bubbleList
    void disableAllSpeechBubbles()
    {
        for (int index = 0; index < comicDataList.Count; index++)
            disableSpeechBubble(index);
    }


    // Disables the speechbubble object referenced by the index
    void disableSpeechBubble(int index)
    {
        var bubble = comicDataList[index].speechBubble;

        if (bubble != null )
            bubble.SetActive(false);
    }


    // Enables the speechbubble object referenced by the index
    void enableSpeechBubble(int index)
    {
        var bubble = comicDataList[index].speechBubble;

        if (bubble != null)
            bubble.SetActive(true);
    }


    // Enables (shows) the current bubble object
    void showCurrentBubble()
    {
        enableSpeechBubble(currentIndex);
    }


    // Disables FollowCursor of the current bubble object
    void unfollowCurrentBubble()
    {
        var currentBubble = getCurrentBubble();

        if (currentBubble != null)
            currentBubble.GetComponent<FollowCursor>().enabled = false;
    }


    //  Updates the speechbubble script for easy access
    void updateCurrentBubbleTextScript()
    {
        var currentBubble = getCurrentBubble();

        if (currentBubble != null)
            currentBubbleTextScript = currentBubble.GetComponent<ComicBubble_SpeechBubble>();
        else
            currentBubbleTextScript = null;
    }


    // Move the bubble, reference by the index, slightly upwards
    IEnumerator moveBubbleToFinalPosition(int bubbleIndex)
    {
        Vector2 target = (Vector2)comicDataList[bubbleIndex].speechBubble.transform.position + new Vector2(0, bubbleMovementDistanceAfterEnding);

        Transform bubbleTransform = comicDataList[bubbleIndex].speechBubble.transform;

        float step = bubbleMovementSpeed * Time.deltaTime;

        while (!Mathf.Approximately(((Vector2)bubbleTransform.position - target).sqrMagnitude, 0))
        {
            bubbleTransform.position = Vector2.MoveTowards(bubbleTransform.position, target, step);
            yield return null;
        }
    }


/**
*  BUBBLE SHADOW STUFF
**/

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
            currentBubbleShadow = Instantiate(speechBubble, comicDataList[currentIndex].strip.GetComponentInChildren<Canvas>().transform) as GameObject;
            currentBubbleShadow.transform.position = Vector2.zero;
            currentBubbleShadow.transform.SetSiblingIndex(0);

            // Delete non needed commponents
            var shadowSprite = currentBubbleShadow.GetComponentInChildren<SpriteRenderer>();
            GameObject shadowObject = shadowSprite.gameObject;
            foreach (Transform child in shadowSprite.transform) GameObject.Destroy(child.gameObject);
            Destroy(shadowSprite.GetComponent<ComicBubble_SpeechBubble>());

            // Add the remaining elements
            shadowSprite.color = bubbleShadowColor;
            shadowSprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            shadowSprite.sortingOrder = 1;
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

/**
* EVENTS STUFF
*/

    // Event for changing to the next bubble
    public void eventBubbleHasEnded()
    {
        hideCurrentElements();

        StartCoroutine(moveBubbleToFinalPosition(currentIndex));

        currentIndex++;

        if (currentIndex < comicDataList.Count)
        {
            showCurrentElements();

            // If the next step doesn't have a speechbubble attached, then it's assumes that the game has finished...
            if (getCurrentBubble() == null)
            {
                microgameEnd();
            }
        }
    }

    //  To finally end the game
    public void microgameEnd()
    {
        print("I finish");
        MicrogameController.instance.setVictory(true);

    }


}

