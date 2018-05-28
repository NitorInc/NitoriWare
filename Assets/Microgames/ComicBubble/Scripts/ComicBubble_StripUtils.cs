using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComicBubble_StripUtils {


    //  To hide all strips (used at the start of the game)
    public static void hideAllStrips(this List<ComicBubble_ComicData> dataList, Color deactivatedStripColor)
    {

        for (int index = 0; index < dataList.Count; index++)
        {
            dataList.sendStripToTheBack(index);

            var strip = dataList[index].getStrip();

            foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.color = deactivatedStripColor;
            }

        }

    }


    //  To show the strip related to the index
    public static void showStrip(this List<ComicBubble_ComicData> dataList, int index)
    {
        dataList.sendStripToTheFront(index);

        var strip = dataList[index].getStrip();

        foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material.color = Color.white;
        }
    }


    //  Changes the order in layer of every sprite, canvas and spritemask range of the strip related to the index, so they don't overlap with the elements of other strip
    public static void sendStripToTheFront(this List<ComicBubble_ComicData> dataList, int index)
    {

        var strip = dataList[index].getStrip();

        var scaling = 100 * (dataList.Count - dataList.getStripFirstIndex(index));


        // Update Spritemask Range
        var sprmask = strip.GetComponentInChildren<SpriteMask>(true);

        if (sprmask.backSortingOrder < 0)
        {
            sprmask.frontSortingOrder = sprmask.frontSortingOrder + scaling;
            sprmask.backSortingOrder = sprmask.backSortingOrder + scaling;

            // Update Canvas sorting order
            var canvas = strip.GetComponentInChildren<Canvas>(true);
            if (canvas != null) canvas.sortingOrder = canvas.sortingOrder + scaling;

            // Update Sprites sorting order
            foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>(true))
                sr.sortingOrder = sr.sortingOrder + scaling;
        }

    }


    //  Changes the order in layer of every sprite, canvas and spritemask range of the strip related to the index, so they don't overlap with the elements of other strip
    public static void sendStripToTheBack(this List<ComicBubble_ComicData> dataList, int index)
    {

        var strip = dataList[index].getStrip();

        var scaling = 100 * (dataList.Count - dataList.getStripFirstIndex(index));

        // Update Spritemask Range
        var sprmask = strip.GetComponentInChildren<SpriteMask>(true);

        if (sprmask.backSortingOrder >= 0)
        {
            sprmask.backSortingOrder = sprmask.backSortingOrder - scaling;
            sprmask.frontSortingOrder = sprmask.frontSortingOrder - scaling;

            // Update Canvas sorting order
            var canvas = strip.GetComponentInChildren<Canvas>(true);
            if (canvas != null) canvas.sortingOrder = canvas.sortingOrder - scaling;

            // Update Sprites sorting order
            foreach (SpriteRenderer sr in strip.GetComponentsInChildren<SpriteRenderer>(true))
                sr.sortingOrder = sr.sortingOrder - scaling;
        }


    }

    private static int getStripFirstIndex(this List<ComicBubble_ComicData> dataList, int index)
    {
        var strip = dataList[index].getStrip();
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].getStrip() == strip)
            {
                return i;
            }
        }
        return -1;
    }
}
