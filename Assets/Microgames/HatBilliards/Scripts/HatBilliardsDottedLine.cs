using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HatBilliardsDottedLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int matchIndex;
    [SerializeField]
    private float indexSizeDropoff = .2f;

    private ParticleSystem particles;

    private Keyframe[] speedOverTimeKeys;

    private Color startColor;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();

        var pSizeModule = particles.sizeOverLifetime;
        speedOverTimeKeys = pSizeModule.size.curve.keys;
        startColor = particles.main.startColor.color;
    }

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
        if (lineRenderer.positionCount < matchIndex + 2)
        {
            transform.localScale = Vector3.zero;
            return;
        }
        else
            transform.localScale = Vector3.one;

        var posA = lineRenderer.GetPosition(matchIndex);
        var posB = lineRenderer.GetPosition(matchIndex + 1);

        transform.position = posA;
        transform.LookAt(posB);
        
        // Particle size curve stuff is pass by object so it's a painful climb to the top to edit them
        var pSizeModule = particles.sizeOverLifetime;
        var totalDistance = (posA - posB).magnitude;
        var sizeModule = pSizeModule.size;
        var sizeCurve = pSizeModule.size.curve;
        var newKeys = sizeCurve.keys;
        for (int i = 0; i < pSizeModule.size.curve.keys.Length; i++)
        {
            newKeys[i].time = speedOverTimeKeys[i].time * totalDistance;
        }
        pSizeModule.size.curve.keys = newKeys;
        sizeCurve.keys = newKeys;
        sizeModule.curve = sizeCurve;
        pSizeModule.size = sizeModule;

        // Also set size and color dropoff for reflection lines
        var size = 1f - (matchIndex * (indexSizeDropoff / (lineRenderer.positionCount - 1)));
        pSizeModule.sizeMultiplier = size;
        var pMain = particles.main;
        var color = startColor;
        color.a *= size;
        pMain.startColor = color;
    }
}
