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
        public UnityEvent unityEvent;
    }


    private Queue<Event> upcomingEvents;
    private Event nextEvent;

    void Start()
    {
        upcomingEvents = new Queue<Event>(events);
        YoumuSlashTimingController.onMusicStart += InvokeNextEvent;
    }

    void InvokeNextEvent()
    {
        if (!upcomingEvents.Any())
            return;

        nextEvent = upcomingEvents.Dequeue();
        Invoke("triggerEvent", (nextEvent.beat - timingData.CurrentBeat) * timingData.BeatDuration);
    }

    void triggerEvent()
    {
        nextEvent.unityEvent.Invoke();
        InvokeNextEvent();
    }
}
