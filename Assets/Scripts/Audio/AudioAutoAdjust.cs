using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automatically adjusts pitch od all audiosources in gameobject, and its children if specified
public class AudioAutoAdjust : MonoBehaviour
{

    [Header("DON'T USE THIS SCRIPT ANYMORE!! Simply set your AudioSource's output to the appropriate channel instead (use Microgame Mixer if this is part of a microgame).")]


    [SerializeField]
    private bool includeChildren;
    [SerializeField]
    private bool tieToTimescale = true, tieToVolumeSettings = true;

    [SerializeField]
    private bool preserveInitialPitch;
    [SerializeField]
    private bool updateEachFrame = true;
    [SerializeField]
    private PrefsHelper.VolumeType volumeType = PrefsHelper.VolumeType.SFX;

    [Range(0f, 1f)]
    [SerializeField]
    private float volumeMult = 1f;
    public float VolumeMult
    {
        get { return volumeMult; }
        set { volumeMult = value; }
    }
    [SerializeField]
    [Range(0f, 1f)]
    private float pitchMult = 1f;
	private AudioSource[] sources;
    public float PitchMult
    {
        get { return pitchMult; }
        set { pitchMult = value; }
    }

    private float[] initialVolumes;
    private float[] initialPitches;
    private float instanceTimeScale, instanceVolumeSetting;

    float Volume => volumeMult;
    float Pitch => pitchMult;

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
        {
            initialPitches = new float[sources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                initialPitches[i] = sources[i].pitch;
            }
            updatePitch();
        }
    }

    void Update()
	{
        if (updateEachFrame)
        {
            if (tieToTimescale && Pitch != instanceTimeScale)
		        updatePitch();
            if (tieToVolumeSettings && Volume != instanceVolumeSetting)
                updateVolume();
        }
	}

    public void updateVolume()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = initialVolumes[i] * Volume;
        }
        instanceVolumeSetting = Volume;
    }
	public void updatePitch()
	{
		for (int i = 0; i < sources.Length; i++)
		{
			sources[i].pitch = Pitch;
            if (preserveInitialPitch)
                sources[i].pitch *= initialPitches[i];
        }
        instanceTimeScale = Pitch;
	}
}
