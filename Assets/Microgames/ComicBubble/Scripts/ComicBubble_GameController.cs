using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        currentBubbleIndex = 0;
        setAllComicBubbleData();
        followActualBubble();
        hideAllPanels();
        showActualPanel();
    }

    // Update is called once per frame
    void Update () {

	}

    void setAllComicBubbleData()
    {
        foreach (ComicBubbleDataCollection d in bubbleList){
            if (d.speechBubble != null )
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

    void hideAllPanels()
    {
        foreach (ComicBubbleDataCollection d in bubbleList)
        {
            foreach (SpriteRenderer sr in d.speechStrip.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.color = deactivatedStripColor;
            }
        }
    }

    void showCurrentBubbleShadow()
    {
        // Destroy the current bubble shadow
        if (currentBubbleShadow != null)
        {
            GameObject.Destroy(currentBubbleShadow);
            currentBubbleShadow = null;
        }

        // Get the speechbubble image
        GameObject speechBubble = bubbleList[currentBubbleIndex].speechBubble;
        GameObject bubbleImage = speechBubble.GetComponentInChildren<Image>().gameObject;

        // Instatiating shadow
        currentBubbleShadow = Instantiate(speechBubble, this.transform) as GameObject;
        currentBubbleShadow.transform.position = Vector2.zero;
        currentBubbleShadow.transform.SetSiblingIndex(0);

        // Delete and add components
        Image shadowImage = currentBubbleShadow.GetComponentInChildren<Image>();
        GameObject shadowObject = shadowImage.gameObject;
        foreach (Transform child in shadowImage.transform) GameObject.Destroy(child.gameObject);
        Destroy(shadowImage.GetComponent<ComicBubble_BubbleTextBoxBehaviour>());
        shadowImage.color = bubbleShadowColor;




        print("Bitch");
    }


    void showActualPanel()
    {
        var currentBubble = bubbleList[currentBubbleIndex];

        showCurrentBubbleShadow();

        foreach (SpriteRenderer sr in currentBubble.speechStrip.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material.color = Color.white;
        }
    }

    // Enable FollowCursor of the bubble object referenced by currentBubbleIndex
    void followActualBubble()
    {
        var currentBubble = bubbleList[currentBubbleIndex];
        currentBubble.speechBubble.transform.position = Input.mousePosition;
        currentBubble.speechBubble.GetComponent<FollowCursor>().enabled = true;
    }

    // Disable FollowCursor of the bubble object referenced by currentBubbleIndex
    void unfollowActualBubble()
    {
        var currentBubble = bubbleList[currentBubbleIndex];
        currentBubble.speechBubble.GetComponent<FollowCursor>().enabled = false;
    }


    // Event for changing to the next bubble
    public void eventShowNextBubble()
    {
        unfollowActualBubble();

        StartCoroutine(moveToFinalPosition(currentBubbleIndex));

        currentBubbleIndex++;

        if (bubbleList[currentBubbleIndex].speechBubble != null)
        {
            followActualBubble();

            showActualPanel();
        }
    }


    IEnumerator moveToFinalPosition(int bubbleIndex)
    {
        Vector2 target = (Vector2) bubbleList[bubbleIndex].speechBubble.transform.position + new Vector2(0, bubbleMovementDistanceAfterEnding);
        


        Transform bubbleTransform = bubbleList[bubbleIndex].speechBubble.transform;
        float step = bubbleMovementSpeed * Time.deltaTime;
        
        while (!Mathf.Approximately(((Vector2) bubbleTransform.position - target).sqrMagnitude, 0))
        {
            print("move fucker");
            bubbleTransform.position = Vector2.MoveTowards(bubbleTransform.position, target, step);
            yield return null;

        }


    }

    // Event for ending the microgame
    public void eventEndMicrogame()
    {
        unfollowActualBubble();

        StartCoroutine(moveToFinalPosition(currentBubbleIndex));

        currentBubbleIndex++;

        showActualPanel();

        MicrogameController.instance.setVictory(true);
    }

}

