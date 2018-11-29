using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComicBubble_BubbleUtils {

    //  This puts the data that the speechbubble script needs in order to function correctly.
    //  This is: The bubble target and the speed in which text will appear
    public static void initializeBubbleGameObjects(this List<ComicBubble_ComicData> dataList)
    {
        foreach (ComicBubble_ComicData data in dataList)
        {
            if (data.getBubble() != null)
            {
                if (data.getTarget() != null)
                {
                    var bubbleScript = data.getBubble().GetComponentInChildren<ComicBubble_SpeechBubble>();
                    bubbleScript.setTargetCharacter(data.getTarget());
                    bubbleScript.setTextSpeed(data.getSpeechSpeed());
                }

                else
                {
                    Debug.Log("No speech target attached to the bubble");
                }
            }
        }
    }


    // Disables all of the speech bubbles contained in bubbleList
    public static void disableAllSpeechBubbles(this List<ComicBubble_ComicData> dataList)
    {
        for (int index = 0; index < dataList.Count; index++)
            dataList.disableSpeechBubble(index);
    }


    // Disables the speechbubble object referenced by the index
    public static void disableSpeechBubble(this List<ComicBubble_ComicData> dataList, int index)
    {
        var bubble = dataList[index].getBubble();

        if (bubble != null)
            bubble.SetActive(false);
    }


    // Enables the speechbubble object referenced by the index
    public static void enableSpeechBubble(this List<ComicBubble_ComicData> dataList, int index)
    {
        var bubble = dataList[index].getBubble();

        if (bubble != null)
            bubble.SetActive(true);
    }


    //  Disables the FollowScript of the bubble referenced by the index
    public static void unfollowSpeechBubble(this List<ComicBubble_ComicData> dataList, int index)
    {
        var bubble = dataList.getBubble(index);
        if (bubble != null)
            bubble.GetComponentInChildren<FollowCursor>().enabled = false;
    }


    //  Disables the animator of the bubble referenced by the index
    public static void disableAnimator(this List<ComicBubble_ComicData> dataList, int index)
    {
        var bubble = dataList.getBubble(index);
        if (bubble != null)
            bubble.GetComponent<Animator>().enabled = false;
    }


    //  Returns the speech bubble script of the bubble referenced by the index
    public static ComicBubble_SpeechBubble getSpeechBubbleScript(this List<ComicBubble_ComicData> dataList, int index)
    {
        var bubble = dataList.getBubble(index);
        if (bubble != null)
            return bubble.GetComponentInChildren<ComicBubble_SpeechBubble>();
        else
            return null;
    }

}
