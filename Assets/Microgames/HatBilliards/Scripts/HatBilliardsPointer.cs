using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HatBilliardsPointer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int matchIndex;

    private Keyframe[] speedOverTimeKeys;

    private void Start()
    {
        HatBilliardsBall.onHit += onHit;
    }

    void onHit()
    {
        gameObject.SetActive(false);
    }

    void Update ()
    {
        if (lineRenderer.positionCount > matchIndex + 1)
        {
            var posA = lineRenderer.GetPosition(matchIndex);
            var posB = lineRenderer.GetPosition(matchIndex + 1);

            transform.position = posA;
            transform.LookAt(posB);
        }
    }
}
