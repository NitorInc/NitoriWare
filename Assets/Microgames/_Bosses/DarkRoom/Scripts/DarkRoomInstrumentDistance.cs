using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomInstrumentDistance : MonoBehaviour
{
    [SerializeField]
    InstrumentRule[] instrumentRules;

    [System.Serializable]
    private class InstrumentRule
    {
        public Vector2 volumeRange;
        public Vector2 distanceRange;
        public DarkRoomMusicController.Instrument instrument;
        public bool fromLeftOnly;
        public Side fromSide;
        public float sideThreshold;
    }

    public enum Side
    {
        Any,
        LeftOfPlayer,
        RightOfPlayer
    }


    
	void Update ()
    {
        foreach (var rule in instrumentRules)
        {
            var distance = ((Vector2)(transform.position - MainCameraSingleton.instance.transform.position)).magnitude;
            var t = Mathf.InverseLerp(rule.distanceRange.x, rule.distanceRange.y, distance);
            var volumeLevel = Mathf.Lerp(rule.volumeRange.x, rule.volumeRange.y, t);
            if (rule.fromSide != Side.Any)
            {
                var xDiff = transform.position.x - MainCameraSingleton.instance.transform.position.x;
                if ((rule.fromSide == Side.LeftOfPlayer && xDiff > rule.sideThreshold)
                    || (rule.fromSide == Side.RightOfPlayer && xDiff < rule.sideThreshold))
                    volumeLevel = 0f;
            }
            DarkRoomMusicController.instance.SetVolumeLevel(rule.instrument, volumeLevel);
        }
	}
}
