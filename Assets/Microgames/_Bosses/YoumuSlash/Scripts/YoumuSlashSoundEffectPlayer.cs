using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashSoundEffectPlayer : MonoBehaviour
{
    public static YoumuSlashSoundEffectPlayer instance;

    [SerializeField]
    private AudioSource audioSourcePrefab;

    List<AudioSource> sourceList;

    float getStereoPan(float panAmount, YoumuSlashBeatMap.TargetBeat.Direction direction) =>
        panAmount * (direction == YoumuSlashBeatMap.TargetBeat.Direction.Left ? -1f : 1f);

    private void Awake()
    {
        instance = this;
        sourceList = new List<AudioSource>();
    }

    public void play(YoumuSlashSoundEffect soundEffect, YoumuSlashBeatMap.TargetBeat.Direction direction = YoumuSlashBeatMap.TargetBeat.Direction.Right)
    {
        foreach (var sound in soundEffect.Sounds)
        {
            AudioSource newSource;
            var reuseAudioSource = sourceList.FirstOrDefault(a => a.panStereo == getStereoPan(sound.PanAmount, direction));
            if (reuseAudioSource != null)
                newSource = reuseAudioSource;
            else
            {
                newSource = Instantiate(audioSourcePrefab, transform);
                newSource.panStereo = getStereoPan(sound.PanAmount, direction);
                sourceList.Add(newSource);
            }
            newSource.PlayOneShot(sound.Clip, sound.Volume);
        }
    }
}
