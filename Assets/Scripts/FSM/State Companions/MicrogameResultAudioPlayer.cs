using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace StageFSM
{
    class MicrogameResultAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource victorySource;
        [SerializeField]
        private AudioSource failureSource;
        [SerializeField]
        private AudioPlaybackController audioPlaybackController;

        double scheduledStartTime;

        private AudioSource GetResultSource(bool victoryStatus) => victoryStatus ? victorySource : failureSource;

        public void SetClip(bool victoryStatus, AudioClip clip, float pitch)
        {
            var source = GetResultSource(victoryStatus);
            source.pitch = pitch;
            source.clip = clip;
        }

        public void SchedulePlayback(double delay)
        {
            SchedulePlayback(true, delay);
            SchedulePlayback(false, delay);
        }

        public void SchedulePlayback(bool victoryStatus, double delay)
        {
            var source = GetResultSource(victoryStatus);
            scheduledStartTime = AudioSettings.dspTime + delay;
            source.PlayScheduled(scheduledStartTime);
            audioPlaybackController.SetAudioTime(scheduledStartTime);
        }

        public void SetPlaybackTime(double time)
        {
            SetPlaybackTimeInternal(time);
            ApplyPlaybackTime(true);
            ApplyPlaybackTime(false);
        }

        public void SetPlaybackTime(bool victoryStatus, double time)
        {
            SetPlaybackTimeInternal(time);
            ApplyPlaybackTime(victoryStatus);
        }

        void SetPlaybackTimeInternal(double amount)
        {
            scheduledStartTime = amount;
        }

        void ApplyPlaybackTime(bool victoryStatus)
        {
            var source = GetResultSource(victoryStatus);
            source.SetScheduledStartTime(scheduledStartTime);
            audioPlaybackController.SetAudioTime(scheduledStartTime);
        }

        public void UpdateClipVolumes(bool victoryStatus)
        {
            if (victoryStatus)
            {
                victorySource.volume = 1f;
                failureSource.volume = 0f;
            }
            else
            {
                failureSource.volume = 1f;
                victorySource.volume = 0f;
            }
        }
    }
}
