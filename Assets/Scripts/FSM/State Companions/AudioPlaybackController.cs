﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace StageFSM
{
    public class AudioPlaybackController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource[] musicSources;

        private int musicSourceIndex = 0;

        public double LastScheduledAudioStartTime { get; private set; }

        public AudioSource CurrentSource => musicSources[musicSourceIndex % musicSources.Length];
        public AudioSource PreviousSource => musicSources[MathHelper.trueMod(musicSourceIndex - 1, musicSources.Length)];
        public bool IsAudioPlaying => CurrentSource.isPlaying && CurrentSource.clip != null;

        public AudioSource GetNextAudioSource()
        {
            // Offset 2+ music sources so one is being used to schedule play while another is being played
            musicSourceIndex++;
            return musicSources[musicSourceIndex % musicSources.Length];
        }

        public void SetTimeToCurrentAudioEnd()
        {
            LastScheduledAudioStartTime += ((double)CurrentSource.timeSamples / (double)CurrentSource.clip.frequency) / (double)CurrentSource.pitch;
        }

        public void ShiftAudioPlaybackTimeInBeats(double beats)
        {
            double delay = beats * (double)StageController.beatLength / Time.timeScale;
            LastScheduledAudioStartTime += delay;
        }

        public void ResetAudioPlaybackTime()
        {
            LastScheduledAudioStartTime = AudioSettings.dspTime;
        }

        public void ScheduleClip(AudioSource source, AudioClip clip, float pitch)
        {
            source.clip = clip;
            source.pitch = pitch;
            source.PlayScheduled(LastScheduledAudioStartTime);
        }
    }
}
