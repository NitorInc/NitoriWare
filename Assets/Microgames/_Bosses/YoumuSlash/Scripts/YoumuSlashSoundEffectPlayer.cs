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
            var reuseAudioSource = sourceList.FirstOrDefault(a => MathHelper.Approximately(Mathf.Abs(a.panStereo), sound.PanAmount, .01f));
            if (reuseAudioSource != null)
                newSource = reuseAudioSource;
            else
            {
                newSource = Instantiate(audioSourcePrefab, transform);
                newSource.panStereo = sound.PanAmount
                    * (direction == YoumuSlashBeatMap.TargetBeat.Direction.Left ? -1f : 1f);
            }
            newSource.PlayOneShot(sound.Clip, sound.Volume);
        }
    }
	
	void Update ()
    {
		
	}
}
