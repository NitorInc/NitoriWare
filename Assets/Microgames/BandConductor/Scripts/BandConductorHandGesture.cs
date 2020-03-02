using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BandConductorHandGesture : MonoBehaviour
{
    [SerializeField]
    protected float progress;
    public float Progress => progress;
    [SerializeField]
    private int gestureIndex;
    public int GestureIndex => gestureIndex;
    [SerializeField]
    protected float maxBeatsLeftForInput = 16f;

    public virtual void ResetGesture()
    {
        progress = 0f;
    }

    public abstract void Update();

    public abstract Vector3 getStartPosition();

    public abstract float GetConductorAnimationPosition();
}
