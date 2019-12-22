using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Microgame Assets/YoumuSlash/Beat Map")]
public class YoumuSlashBeatMap : ScriptableObject
{
    [SerializeField]
    private YoumuSlashTargetType defaultTypeData;
    public YoumuSlashTargetType DefaultTypeData => defaultTypeData;

    [SerializeField]
    private List<TargetBeat> targetBeats;
    public List<TargetBeat> TargetBeats => targetBeats;
    
    [System.Serializable]
    public class TargetBeat
    {
        public const float LaunchBeatDuration = 2f;

        public YoumuSlashTarget launchInstance { get; set; }
        public bool slashed { get; set; }
        public void resetFields(YoumuSlashBeatMap beatMap)
        {
            launchInstance = null;
            slashed = false;
            defaultTypeData = beatMap.DefaultTypeData;
        }

        [SerializeField]
        private string notes;

        [SerializeField]
        private float launchBeat;
        public float LaunchBeat => launchBeat;
        public float HitBeat => launchBeat + LaunchBeatDuration;

        [SerializeField]
        private Direction hitDirection;
        public Direction HitDirection => hitDirection;

        [SerializeField]
        private YoumuSlashTargetType typeData;
        public YoumuSlashTargetType TypeData => typeData != null ? typeData : defaultTypeData;
        private YoumuSlashTargetType defaultTypeData;

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
            Passed,
            Slashed
        }

        public TimeState getTimeState(float beat, float maxHitTime)
        {
            if (slashed)
                return TimeState.Slashed;
            else if (beat < launchBeat)
                return TimeState.Pending;
            else if (beat <= HitBeat + maxHitTime)
                return TimeState.Active;
            else
                return TimeState.Passed;
        }

        public bool isInHitRange(float beat, float minHitTime, float maxHitTime)
        {
            return beat >= HitBeat + minHitTime && beat <= HitBeat + maxHitTime;
        }
    }

    public void initiate()
    {
        foreach (var target in targetBeats)
        {
            target.resetFields(this);
        }
    }

    public TargetBeat getFirstActiveTarget(float beat, float maxHitTime)
    {
        foreach (var target in TargetBeats)
        {
            var targetState = target.getTimeState(beat, maxHitTime);
            if (targetState == TargetBeat.TimeState.Active)
                return target;
            else if (targetState == TargetBeat.TimeState.Pending)
                return null;
        }
        return null;
    }

    public TargetBeat getLastActiveTarget(float beat, float maxHitTime)
    {
        TargetBeat lastFoundTarget = null;
        foreach (var target in TargetBeats)
        {
                var targetState = target.getTimeState(beat, maxHitTime);
                if (targetState == TargetBeat.TimeState.Active)
                    lastFoundTarget = target;
                else if (targetState == TargetBeat.TimeState.Pending)
                    return lastFoundTarget;
        }
        return lastFoundTarget;
    }

    public TargetBeat getFirstHittableTarget(float beat, float minHitTime, float maxHitTime, TargetBeat.Direction direction = TargetBeat.Direction.Any)
    {
        foreach (var target in TargetBeats)
        {
            var targetState = target.getTimeState(beat, maxHitTime);
            if (targetState == TargetBeat.TimeState.Active && target.isInHitRange(beat, minHitTime, maxHitTime)
                && (direction == TargetBeat.Direction.Any || direction == target.HitDirection))
                return target;
            else if (targetState == TargetBeat.TimeState.Pending)
                return null;
        }
        return null;
    }

    public TargetBeat getNextLaunchingTarget(float beat)
    {
        return TargetBeats.FirstOrDefault(a => a.LaunchBeat >= beat);
    }

}
