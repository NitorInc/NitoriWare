using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StageFSM
{
    public class DirectorPlaybackController : MonoBehaviour
    {
        public double time { get; set; }
        public PlayableAsset AssetToSwap { get; set; }
        public bool ContinuePlayableAfterEnd { get; set; }

        private PlayableDirector director;
        private List<INotification> markersPlayedOnPlayable;

        private bool finishedPlayable;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
            markersPlayedOnPlayable = new List<INotification>();
        }

        public void ResetPlayback()
        {
            director.time = 0d;
            director.playableAsset = null;
            time = 0d;
        }

        private void LateUpdate()
        {
            if (AssetToSwap != null)
            {
                if (director.playableAsset != null)
                {
                    time -= director.playableAsset.duration;
                    ManualSetWithNotifications(director, director.playableAsset.duration);
                }
                else
                    time = 0d;
                director.Stop();
                director.playableAsset = AssetToSwap;
                director.Play();
                AssetToSwap = null;
                finishedPlayable = false;
                markersPlayedOnPlayable.Clear();
            }

            if (director.playableAsset != null && !finishedPlayable)
            {
                if (!ContinuePlayableAfterEnd && time > director.playableAsset.duration)
                {
                    finishedPlayable = true;
                    time = director.playableAsset.duration;
                }
                ManualSetWithNotifications(director, time > 0d ? time : 0d);
                director.time = time;
            }
        }

        void ManualSetWithNotifications(PlayableDirector director, double time)
        {
            if (director == null || !director.playableGraph.IsValid() || director.timeUpdateMode != DirectorUpdateMode.Manual)
                return;

            var oldTime = director.time;
            director.time = time;
            for (int i = 0; i < director.playableGraph.GetOutputCount(); i++)
            {
                var output = director.playableGraph.GetOutput(i);
                var playable = output.GetSourcePlayable().GetInput(i);
                var track = output.GetReferenceObject() as TrackAsset;
                if (track == null)
                    continue;

                var markers = track.GetMarkers().OfType<Marker>()
                    .OrderBy(a => a.time);
                foreach (var m in markers)
                {
                    if (!(m is INotification))
                        continue;

                    bool fire = ((m.time >= oldTime && m.time < time) || (m.time > time && m.time <= oldTime))
                        && !markersPlayedOnPlayable.Contains(m as INotification);
                    if (fire)
                    {
                        output.PushNotification(playable, m as INotification);
                        markersPlayedOnPlayable.Add(m as INotification);
                    }
                }
            }
            director.Evaluate();
        }
    }
}