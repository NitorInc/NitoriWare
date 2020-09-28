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
    private AudioMixer gameplayMixer;
    public AudioMixer GameplayMixer => gameplayMixer;
    [SerializeField]
    private AudioMixer microgameMixer;
    public AudioMixer MicrogameMixer => microgameMixer;

    // Needed until we clean up legacy uses of AudioAutoAdjust
    [SerializeField]
    private AudioMixerGroup microgameSFXGroup;
    public AudioMixerGroup MicrogameSFXGroup => microgameSFXGroup;
    [SerializeField]
    private AudioMixerGroup microgameMusicGroup;
    public AudioMixerGroup MicrogameMusicGroup => microgameMusicGroup;

    private void Awake()
    {
        instance = this;
        foreach (PrefsHelper.VolumeType volumeType in Enum.GetValues(typeof(PrefsHelper.VolumeType)))
        {
            SetVolume(volumeType, PrefsHelper.GetVolumeSetting(volumeType));
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
                gameplayMixer.SetFloat("MusicVolume", dbLevel);
                microgameMixer.SetFloat("MusicVolume", dbLevel);
                break;
            case (PrefsHelper.VolumeType.SFX):
                masterMixer.SetFloat("SFXVolume", dbLevel);
                gameplayMixer.SetFloat("SFXVolume", dbLevel);
                microgameMixer.SetFloat("SFXVolume", dbLevel);
                break;
            case (PrefsHelper.VolumeType.Voice):
                masterMixer.SetFloat("VoiceVolume", dbLevel);
                gameplayMixer.SetFloat("VoiceVolume", dbLevel);
                break;
        }
    }
}
