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
    }

    
	void Update ()
    {
        foreach (var rule in instrumentRules)
        {
            var distance = ((Vector2)(transform.position - MainCameraSingleton.instance.transform.position)).magnitude;
            var t = Mathf.InverseLerp(rule.distanceRange.x, rule.distanceRange.y, distance);
            var volumeLevel = Mathf.Lerp(rule.volumeRange.x, rule.volumeRange.y, t);
            DarkRoomMusicController.instance.SetVolumeLevel(rule.instrument, volumeLevel);
        }
	}
}
