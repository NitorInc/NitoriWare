using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HatBilliardsMouseTracker : MonoBehaviour
{

    HatBilliardsCue cue;

    [SerializeField]
    private int reflectCount = 2;
    [SerializeField]
    private float maxRaycastDistance = 10f;
    [SerializeField]
    private LayerMask bankMask;
    [SerializeField]
    private LayerMask mousePositionMask;

    [SerializeField]
    Transform mouseLocation;

    [SerializeField]
    LineRenderer guideLine;

    private void Start()
    {
        guideLine.SetPosition(0, guideLine.transform.position);
        HatBilliardsBall.onHit += onHit;
    }

    void onHit()
    {
        enabled = false;
    }

    void Update ()
    {
        // Determine mouse location
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit mouseHit;
        Physics.Raycast (ray, out mouseHit, Mathf.Infinity, mousePositionMask.value);

        mouseLocation.position = new Vector3 (mouseHit.point.x, transform.position.y, mouseHit.point.z);
        RaycastHit hitInfo;
        
        // Calculate initial angle
        var currentPos = transform.position;
        var direction = mouseLocation.position - currentPos;

        // Calculate bank angles
        Vector3 targetPos = mouseLocation.position;
        var travelPoints = new List<Vector3>();
        travelPoints.Add(transform.position);
        for (int i = 0; i < reflectCount + 1; i++)
        {
            Debug.DrawLine(currentPos, currentPos + (direction.normalized));
            if (Physics.Raycast(currentPos, direction, out hitInfo, maxRaycastDistance, bankMask.value))
            {
                targetPos = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
                var reflectDirection = Vector3.Reflect(direction, hitInfo.normal);
                reflectDirection.y = 0f;
                direction = reflectDirection;
                currentPos = targetPos;

                if (hitInfo.collider.tag.ToString().Equals("MicrogameTag1"))
                    i = reflectCount + 1;
            }
            else
            {
                targetPos = currentPos + (direction * maxRaycastDistance);
                i = reflectCount;
            }
            targetPos.y = transform.position.y;
            travelPoints.Add(targetPos);
        }

        // Set points
        guideLine.positionCount = travelPoints.Count;
        guideLine.SetPositions(travelPoints.ToArray());
    }
}
