using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private AudioMixer masterMixer;
    public AudioMixer MasterMixer => masterMixer;
    [SerializeField]
    private AudioMixer microgameMixer;
    public AudioMixer MicrogameMixer => microgameMixer;

    private void Awake()
    {
        instance = this;
        foreach (PrefsHelper.VolumeType volumeType in Enum.GetValues(typeof(PrefsHelper.VolumeType)))
        {
            SetVolume(volumeType, PrefsHelper.getVolumeRaw(volumeType));
        }
    }

    public void SetVolume(PrefsHelper.VolumeType volumeType, float volumeLevel)
    {
        var dbLevel = AudioHelper.VolumeLevelToDecibals(volumeLevel);
        switch(volumeType)
        {
            case (PrefsHelper.VolumeType.Master):
                masterMixer.SetFloat("MasterVolume", dbLevel);
                break;
            case (PrefsHelper.VolumeType.Music):
                masterMixer.SetFloat("MusicVolume", dbLevel);
                microgameMixer.SetFloat("MusicVolume", dbLevel);
                break;
            case (PrefsHelper.VolumeType.SFX):
                masterMixer.SetFloat("MusicVolume", dbLevel);
                microgameMixer.SetFloat("MusicVolume", dbLevel);
                break;
            case (PrefsHelper.VolumeType.Voice):
                masterMixer.SetFloat("VoiceVolume", dbLevel);
                break;
        }
    }
}
