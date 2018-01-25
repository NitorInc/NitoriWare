﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automatically adjusts pitch od all audiosources in gameobject, and its children if specified
public class AudioAutoAdjust : MonoBehaviour
{
  [SerializeField]
  private bool includeChildren, tieToTimescale = true, tieToVolumeSettings = true;
  [SerializeField]
  private PrefsHelper.VolumeType volumeType = PrefsHelper.VolumeType.SFX;
  private AudioSource[] sources;

  private float[] initialVolumes;
  private float instanceTimeScale, instanceVolumeSetting;

  void Awake()
  {
    sources = includeChildren ? GetComponentsInChildren<AudioSource>() : GetComponents<AudioSource>();
    if (tieToVolumeSettings)
    {
      initialVolumes = new float[sources.Length];
      for (int i = 0; i < sources.Length; i++)
      {
        initialVolumes[i] = sources[i].volume;
      }
      updateVolume();
    }
    if (tieToTimescale)
      updatePitch();
  }

  void Update()
  {
    if (tieToTimescale && Time.timeScale != instanceTimeScale)
      updatePitch();
    if (tieToVolumeSettings && PrefsHelper.getVolume(volumeType) != instanceVolumeSetting)
      updateVolume();
  }

  public void updateVolume()
  {
    for (int i = 0; i < sources.Length; i++)
    {
      sources[i].volume = initialVolumes[i] * PrefsHelper.getVolume(volumeType);
    }
    instanceVolumeSetting = PrefsHelper.getVolume(volumeType);
  }
  public void updatePitch()
  {
    for (int i = 0; i < sources.Length; i++)
    {
      sources[i].pitch = Time.timeScale;
    }
    instanceTimeScale = Time.timeScale;
  }
}
