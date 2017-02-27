using System.Collections.Generic;
using UnityEngine;

public class TouhouSortZoneManager : MonoBehaviour
{
    public TouhouSortDropZone[] zones;

    public Dictionary<TouhouSortSorter.Style, int> indicatorIcons;

    public void SetZoneAttributes(int index, Sprite sprite, string style, bool invert)
    {
        TouhouSortDropZone zone = zones[index];

        zone.SetCategory(style, invert, sprite);
    }

}
