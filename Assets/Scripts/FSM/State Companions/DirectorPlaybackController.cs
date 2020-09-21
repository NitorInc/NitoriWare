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

        private PlayableDirector director;
        public PlayableAsset AssetToSwap;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
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
            }

            if (director.playableAsset != null)
                ManualSetWithNotifications(director, time > 0d ? time : 0d);
        }

        public static void ManualSetWithNotifications(PlayableDirector director, double time)
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

                    bool fire = (m.time >= oldTime && m.time < time) || (m.time > time && m.time <= oldTime);
                    if (fire)
                        output.PushNotification(playable, m as INotification);
                }
            }
            director.Evaluate();
        }
    }
}