using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsMouseTracker : MonoBehaviour
{
    HatBilliardsCue cue;

    [SerializeField]
    Transform start;
    [SerializeField]
    Transform mouseLocation;
    [SerializeField]
    Transform target;

    [SerializeField]
    LineRenderer guideLine;

    void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast (ray, out hit);

        mouseLocation.position = new Vector3 (hit.point.x, transform.position.y, hit.point.z);

        RaycastHit sideHit;

        Vector3 fromPosition = start.position;
        Vector3 toPosition = mouseLocation.position;
        Vector3 direction = toPosition - fromPosition;
        Ray shot = new Ray (start.position, direction);

        Physics.Raycast (shot, out sideHit);
        target.position = new Vector3 (sideHit.point.x, transform.position.y, sideHit.point.z);

        guideLine.SetPosition (1, new Vector3 (target.localPosition.x, target.localPosition.z, 0));
        guideLine.SetPosition (2, new Vector3 (0, target.localPosition.z * 2, 0));
    }
}
