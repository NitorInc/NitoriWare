using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[ExecuteInEditMode]
public class YoumuSlashEventMap : MonoBehaviour
{
    [SerializeField]
	private YoumuSlashTimingData timingData;
	[SerializeField]
	private bool sort;
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
		if (!Application.isPlaying)
			return;
        upcomingEvents = new Queue<Event>(events);
        YoumuSlashPlayerController.onFail += onFail;
    }

    void Update()
	{
		if (!Application.isPlaying)
		{
			if (sort)
			{
				events = events.OrderBy (a => a.beat).ToList ();
				sort = false;
			}
				
			return;
		}
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
