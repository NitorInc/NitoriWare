using System.Collections.Generic;
using UnityEngine;

public class TouhouSortDropZone : MonoBehaviour
{
    // A zone that a sortable must be sorted into

    //public SpriteRenderer icon;
    

    List<TouhouSortSortable> targets;
    
    public void SetTargets(List<TouhouSortSortable> touhous)
    {
        targets = touhous;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        TouhouSortSortable sortable = other.GetComponentInParent<TouhouSortSortable>();
        if (sortable != null && targets.Contains(sortable))
            sortable.InWinZone = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        TouhouSortSortable sortable = other.GetComponentInParent<TouhouSortSortable>();
        if (sortable != null && targets.Contains(sortable))
            sortable.InWinZone = false;
    }

}
