using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class YoumuSlashEventMap : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private List<Event> events;

    [System.Serializable]
    public class Event
    {
        public string description;
        public float beat;
        public bool debugPause;
        public UnityEvent unityEvent;
    }


    private Queue<Event> upcomingEvents;

    void Start()
    {
        upcomingEvents = new Queue<Event>(events);
        YoumuSlashPlayerController.onFail += onFail;
    }

    void Update()
    {
        if (!upcomingEvents.Any())
            return;
        else if (timingData.CurrentBeat >= upcomingEvents.Peek().beat)
        {
            var newEvent = upcomingEvents.Dequeue();
            newEvent.unityEvent.Invoke();
            if (newEvent.debugPause)
                Debug.Break();
        }
    }

    void onFail()
    {
        enabled = false;
    }
}
