using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ComicBubble_GameController : MonoBehaviour {

    [System.Serializable]
    class ComicBubbleDataCollection
    {
        public GameObject speechBubble;
        public GameObject speechTarget;
        public GameObject speechStrip;
        public float speechSpeed;
    }

    [SerializeField]
    List<ComicBubbleDataCollection> bubbleList;

    [SerializeField]
    Color deactivatedStripColor;

    [SerializeField]
    float bubbleMovementDistanceAfterEnding;

    [SerializeField]
    float bubbleMovementSpeed;



    [SerializeField]
    Color bubbleShadowColor;

    int currentBubbleIndex;

    GameObject currentBubbleShadow;



    // Use this for initialization
    void Start() {

        // Set the text speed for each bubble
        setAllComicBubbleData();

        // Initalize the bubble index in 0
        currentBubbleIndex = 0;

        // Hide every panel at first
        hideAllStrips();

        // Show the current elements
        showCurrentElements();

    }

    // Update is called once per frame
    void Update() {

    }

    void setAllComicBubbleData()
    {
        foreach (ComicBubbleDataCollection d in bubbleList) {
            if (d.speechBubble != null)
            {
                if (d.speechTarget != null)
                {
                    var bubbleScript = d.speechBubble.GetComponent<ComicBubble_BubbleTextBoxBehaviour>();
                    bubbleScript.setTarget(d.speechTarget);
                    bubbleScript.setTextSpeed(d.speechSpeed);
                }

                else
                {
                    Debug.Log("No speech target attached to the bubble");
                }
            }
        }
    }


    //  ALL ELEMENTS    
    void showCurrentElements()
    {
        showCurrentStrip();

        showCurrentBubble();

        showCurrentBubbleShadow();

    }

    void hideCurrentElements()
    {
        unfollowCurrentBubble();

        hideCurrentBubbleShadow();
    }


    //  PANEL STUFF

    void showCurrentStrip()
    {

        sendPreviousStripToTheBack();
        sendCurrentStripToTheTop();

        var currentBubble = bubbleList[currentBubbleIndex];

        foreach (SpriteRenderer sr in currentBubble.speechStrip.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material.color = Color.white;
        }
    }

    void hideAllStrips()
    {
        int index = 0;
        foreach (ComicBubbleDataCollection d in bubbleList)
        {
            sendStripToTheBack(index);
            foreach (SpriteRenderer sr in d.speechStrip.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.color = deactivatedStripColor;
            }
            index++;
        }
    }

    void sendCurrentStripToTheTop()
    {
        sendStripToTheFront(currentBubbleIndex);
    }

    void sendPreviousStripToTheBack()
    {
        if (currentBubbleIndex > 0)
            sendStripToTheBack(currentBubbleIndex-1);
    }


    void sendStripToTheFront(int index)
    {

        print("send strip " + index + " to the front");
        var strip = bubbleList[index].speechStrip;

        // Update Spritemask Range
        var sprmask = strip.GetComponentInChildren<SpriteMask>();
        sprmask.frontSortingOrder = sprmask.frontSortingOrder + 100;
        sprmask.backSortingOrder = sprmask.backSortingOrder + 100;

        // Update Canvas sorting order
        var canvas = strip.GetComponentInChildren<Canvas>();
        canvas.sortingOrder = canvas.sortingOrder + 100;

        // Update Sprites sorting order
        foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = sr.sortingOrder + 100;
    }

    void sendStripToTheBack(int index)
    {
        print("send strip " + index + " to the bacl");

        var strip = bubbleList[index].speechStrip;

        // Update Spritemask Range
        var sprmask = strip.GetComponentInChildren<SpriteMask>();
        sprmask.backSortingOrder = sprmask.backSortingOrder - 100;
        sprmask.frontSortingOrder = sprmask.frontSortingOrder - 100;

        // Update Canvas sorting order
        var canvas = strip.GetComponentInChildren<Canvas>();
        canvas.sortingOrder = canvas.sortingOrder - 100;

        // Update Sprites sorting order
        foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
            sr.sortingOrder = sr.sortingOrder - 100;
    }


    //  BUBBLE STUFF

    // Enable FollowCursor of the bubble object referenced by currentBubbleIndex
    void showCurrentBubble()
    {
        var currentBubble = bubbleList[currentBubbleIndex];
        currentBubble.speechBubble.transform.position = Input.mousePosition;
        currentBubble.speechBubble.GetComponent<FollowCursor>().enabled = true;
    }

    // Disable FollowCursor of the bubble object referenced by currentBubbleIndex
    void unfollowCurrentBubble()
    {
        var currentBubble = bubbleList[currentBubbleIndex];
        currentBubble.speechBubble.GetComponent<FollowCursor>().enabled = false;
    }

    // Move the bubble specified by the index slightly upwards
    IEnumerator moveBubbleToFinalPosition(int bubbleIndex)
    {
        Vector2 target = (Vector2)bubbleList[bubbleIndex].speechBubble.transform.position + new Vector2(0, bubbleMovementDistanceAfterEnding);

        Transform bubbleTransform = bubbleList[bubbleIndex].speechBubble.transform;
        float step = bubbleMovementSpeed * Time.deltaTime;

        while (!Mathf.Approximately(((Vector2)bubbleTransform.position - target).sqrMagnitude, 0))
        {
            bubbleTransform.position = Vector2.MoveTowards(bubbleTransform.position, target, step);
            yield return null;
        }
    }


    //  BUBBLE SHADOW SUTFF

    void showCurrentBubbleShadow()
    {
        hideCurrentBubbleShadow();

        // Get the speechbubble image
        GameObject speechBubble = bubbleList[currentBubbleIndex].speechBubble;
        GameObject bubbleImage = speechBubble.GetComponentInChildren<SpriteRenderer>().gameObject;

        // Instatiating shadow
        currentBubbleShadow = Instantiate(speechBubble, this.GetComponentInChildren<Canvas>().transform) as GameObject;
        currentBubbleShadow.transform.position = Vector2.zero;
        currentBubbleShadow.transform.SetSiblingIndex(0);

        // Delete and add components
        var shadowSprite = currentBubbleShadow.GetComponentInChildren<SpriteRenderer>();
        GameObject shadowObject = shadowSprite.gameObject;
        foreach (Transform child in shadowSprite.transform) GameObject.Destroy(child.gameObject);
        Destroy(shadowSprite.GetComponent<ComicBubble_BubbleTextBoxBehaviour>());
        shadowSprite.color = bubbleShadowColor;
        shadowSprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        shadowSprite.sortingOrder = 0;
    }

    void hideCurrentBubbleShadow()
    {
        // Destroy the current bubble shadow
        if (currentBubbleShadow != null)
        {
            GameObject.Destroy(currentBubbleShadow);
            currentBubbleShadow = null;
        }
    }

//  EVENTS STUFF

    // Event for changing to the next bubble

    public void eventShowNextBubble()
    {
        hideCurrentElements();

        StartCoroutine(moveBubbleToFinalPosition(currentBubbleIndex));

        currentBubbleIndex++;

        if (currentBubbleIndex < bubbleList.Count &&
            bubbleList[currentBubbleIndex].speechBubble != null)
        {
            showCurrentElements();
        }
    }

    // Event for ending the microgame
    public void eventEndMicrogame()
    {
        unfollowCurrentBubble();

        StartCoroutine(moveBubbleToFinalPosition(currentBubbleIndex));

        currentBubbleIndex++;

        showCurrentStrip();

        MicrogameController.instance.setVictory(true);
    }

}

