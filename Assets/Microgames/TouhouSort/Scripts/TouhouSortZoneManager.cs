using System.Collections.Generic;
using UnityEngine;

public class TouhouSortZoneManager : MonoBehaviour
{
    [SerializeField]
    private TouhouSortDropZone leftZone;

    [SerializeField]
    private TouhouSortDropZone rightZone;
    
    public Dictionary<TouhouSortSorter.Style, int> indicatorIcons;

    private void Start()
    {
        var flip = MathHelper.randomBool();
        if (flip)
        {
            var holdPosition = leftZone.transform.position;
            leftZone.transform.position = rightZone.transform.position;
            rightZone.transform.position = holdPosition;
            leftZone.transform.localScale = new Vector3(-1f, 1f, 1f);
            rightZone.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void SetTargets(List<TouhouSortSortable> leftTargets, List<TouhouSortSortable> rightTargets)
    {
        leftZone.SetTargets(leftTargets);
        rightZone.SetTargets(rightTargets);
        
    }

}
