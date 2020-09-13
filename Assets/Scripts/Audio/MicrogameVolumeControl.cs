using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MicrogameVolumeControl : MonoBehaviour
{
    [SerializeField]
    private AudioMixer microgameMixer;

    [SerializeField]
    private Mode mode;
    [SerializeField]
    private float volume;

    private enum Mode
    {
        decibals,
        volumeLevel
    }

    void LateUpdate()
    {
        float oldVolume;
        microgameMixer.GetFloat("MasterVolume", out oldVolume);
        if (mode == Mode.volumeLevel)
            oldVolume = AudioHelper.DecibalsToVolumeLevel(oldVolume);
        if (!MathHelper.Approximately(volume, oldVolume, .001f))
        {
            var newVolume = volume;
            if (mode == Mode.volumeLevel)
                newVolume = AudioHelper.VolumeLevelToDecibals(newVolume);
            microgameMixer.SetFloat("MasterVolume", newVolume);
        }
    }

}
