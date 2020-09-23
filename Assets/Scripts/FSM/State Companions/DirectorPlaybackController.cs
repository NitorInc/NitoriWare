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

        private PlayableDirector director;
        private bool FinishedPlayingAsset;
        private List<INotification> markersPlayedOnPlayable;

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
                    ManualSetWithNotifications(director, director.playableAsset.duration + 1d);
                }
                else
                    time = 0d;
                director.Stop();
                director.playableAsset = AssetToSwap;
                director.Play();
                AssetToSwap = null;
                FinishedPlayingAsset = false;
                markersPlayedOnPlayable.Clear();
            }

            if (director.playableAsset != null && !FinishedPlayingAsset)
            {
                ManualSetWithNotifications(director, time > 0d ? time : 0d);
                if (time >= director.playableAsset.duration)
                    FinishedPlayingAsset = true;
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

                foreach (var m in track.GetMarkers().OfType<Marker>())
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