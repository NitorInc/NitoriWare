using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashSoundEffectPlayer : MonoBehaviour
{
    public static YoumuSlashSoundEffectPlayer instance;

    [SerializeField]
    private AudioSource audioSourcePrefab;

    float getStereoPan(float panAmount, YoumuSlashBeatMap.TargetBeat.Direction direction) =>
        panAmount * (direction == YoumuSlashBeatMap.TargetBeat.Direction.Left ? -1f : 1f);

    private void Awake()
    {
        instance = this;
    }

    public void play(YoumuSlashSoundEffect soundEffect, YoumuSlashBeatMap.TargetBeat.Direction direction)
        => playScheduled(soundEffect, direction, 0f);

    public void playScheduled(YoumuSlashSoundEffect soundEffect, YoumuSlashBeatMap.TargetBeat.Direction direction, float time)
    {
        foreach (var sound in soundEffect.Sounds)
        {
            var newSource = Instantiate(audioSourcePrefab, transform);
            newSource.pitch = MathHelper.randomRangeFromVector(sound.PitchRange);
            newSource.panStereo = getStereoPan(sound.PanAmount, direction);
            if (sound.PanSpeed != 0f)
                newSource.GetComponent<YoumuSlashMovingAudioSource>().setSpeed(sound.PanSpeed * (direction == YoumuSlashBeatMap.TargetBeat.Direction.Left ? -1f : 1f));
            newSource.clip = sound.Clip;
            if (time <= 0f)
                newSource.Play();
            else
                AudioHelper.playScheduled(newSource, time);
        }

    }

}
