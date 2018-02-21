using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBubble_GameController : MonoBehaviour {

    [System.Serializable]
    class ComicBubbleDataCollection
    {
        public GameObject speechBubble;
        public GameObject speechTarget;
        public GameObject speechStrip;
        public float speechSpeed;
        public Vector2 finalPosition;

    }

    [SerializeField]
    List<ComicBubbleDataCollection> bubbleList;

    [SerializeField]
    Color deactivatedStripColor;

    [SerializeField]
    float bubbleMovementSpeed;

    int currentBubbleIndex;

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

    void showActualPanel()
    {
        var currentBubble = bubbleList[currentBubbleIndex];
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
        Vector2 target = bubbleList[bubbleIndex].finalPosition;
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

