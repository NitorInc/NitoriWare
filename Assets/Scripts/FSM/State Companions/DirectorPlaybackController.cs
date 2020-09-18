using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace StageFSM
{
    public class DirectorPlaybackController : MonoBehaviour
    {
        public float time { get; set; }

        private PlayableDirector director;
        public PlayableAsset AssetToSwap;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        public void PlayNewPlayable(PlayableAsset newPlayable)
        {
            if (director.playableAsset != null)
            {
                director.time = director.playableAsset.duration;
                director.Evaluate();
            }
            director.playableAsset = newPlayable;
            director.Play();
            time = 0f;
            director.time = 0f;
            director.Evaluate();
        }

        private void LateUpdate()
        {
            if (AssetToSwap != null)
            {
                if (director.playableAsset != null)
                {
                    time = (float)director.playableAsset.duration - time;
                    director.time = director.playableAsset.duration + 100f;
                    director.Evaluate();
                    director.Stop();
                }
                director.playableAsset = AssetToSwap;
                director.Play();
                director.time = 0d;
                //director.Evaluate();
                AssetToSwap = null;
            }

            if (director.playableAsset != null)
            {
                director.Evaluate();
            }
        }
    }
}