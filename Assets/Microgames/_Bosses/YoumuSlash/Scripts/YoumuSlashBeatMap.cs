using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashBeatMap : ScriptableObject
{

    [SerializeField]
    private List<TargetBeat> targetBeats;
    public List<TargetBeat> TargetBeats
    {
        get { return targetBeats; }
    }
    
    public class TargetBeat
    {
        [SerializeField]
        private float launchBeat;
        public float LaunchBeat
        {
            get { return launchBeat; }
        }

        [SerializeField]
        private Direction hitDirection;
        public Direction HitDirection
        {
            get { return hitDirection; }
        }

        [SerializeField]
        private GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
        }
        
        public enum Direction
        {
            Left,
            Right,
            Any
        }

        public enum TimeState
        {
            Pending,
            Active,
            Passed
        }

        public TimeState getTimeState(float beat, float maxHitTime)
        {
            if (beat < launchBeat)
                return TimeState.Pending;
            else if (beat <= launchBeat + maxHitTime)
                return TimeState.Active;
            else
                return TimeState.Passed;
        }

        public bool isInHitRange(float beat, float minHitTime, float maxHitTime)
        {
            return beat >= launchBeat - minHitTime && beat <= launchBeat + maxHitTime;
        }
    }

    public TargetBeat getFirstActiveTarget(float beat, float maxHitTime)
    {
        foreach (var target in TargetBeats)
        {
            var targetState = target.getTimeState(beat, maxHitTime);
            if (targetState == TargetBeat.TimeState.Active)
                return target;
            else if (targetState == TargetBeat.TimeState.Passed)
                return null;
        }
        return null;
    }

    public TargetBeat getLastActiveTarget(float beat, float maxHitTime)
    {
        TargetBeat lastFoundTarget = null;
        foreach (var target in TargetBeats)
        {
            if (target.LaunchBeat < beat)
            {
                var targetState = target.getTimeState(beat, maxHitTime);
                if (targetState == TargetBeat.TimeState.Active)
                    lastFoundTarget = target;
                else if (targetState == TargetBeat.TimeState.Passed)
                    return lastFoundTarget;
            }
        }
        return lastFoundTarget;
    }

    public TargetBeat getFirstHittableTarget(float beat, float minHitTime, float maxHitTime, TargetBeat.Direction direction = TargetBeat.Direction.Any)
    {
        foreach (var target in TargetBeats)
        {
            if (target.isInHitRange(beat, minHitTime, maxHitTime)
                && (direction == TargetBeat.Direction.Any || direction == target.HitDirection))
                return target;
            else if (target.getTimeState(beat, maxHitTime) == TargetBeat.TimeState.Passed)
                return null;
        }
        return null;
    }

}
