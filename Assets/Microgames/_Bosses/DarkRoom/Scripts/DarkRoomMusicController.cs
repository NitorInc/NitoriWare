using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DarkRoomMusicController : MonoBehaviour
{
    public static DarkRoomMusicController instance;

    [SerializeField]
    private float volumeUpLerpSpeed = 1f;
    [SerializeField]
    private float volumeDownLerpSpeed = .5f;
    [SerializeField]
    private float volumeMult;
    [SerializeField]
    private AudioSource baseSource;

    public enum Instrument
    {
        ArpBass,
        Toms
    }
    private const int InstrumentCount = 2;

    private AudioAutoAdjust[] instrumentSources;
    private float[] volumeLevels;
    private float[] initialVolumes;


    void Awake ()
    {
        instance = this;
        
        instrumentSources = Enumerable.Range(0, InstrumentCount)
            .Select(a => transform.Find(((Instrument)a).ToString())
                .GetComponent<AudioAutoAdjust>())
            .ToArray();
        initialVolumes = instrumentSources.Select(a => a.VolumeMult).ToArray();
        volumeLevels = instrumentSources.Select(a => 0f).ToArray();
    }

	void LateUpdate ()
    {
        for (int i = 0; i < instrumentSources.Length; i++)
        {
            var source = instrumentSources[i];
            var current = source.VolumeMult;
            var goal = volumeLevels[i];
            var goalVolume = volumeLevels[i] * initialVolumes[i] * volumeMult;
            source.VolumeMult = Mathf.MoveTowards(source.VolumeMult, goalVolume,
                ((source.VolumeMult < goalVolume) ? volumeUpLerpSpeed : volumeDownLerpSpeed) * Time.deltaTime);


                //if (MicrogameController.instance.isDebugMode() && Input.GetKeyDown(KeyCode.S))
                //    source.pitch *= 4f;
                //if (MicrogameController.instance.isDebugMode() && Input.GetKeyUp(KeyCode.S))
                //    source.pitch /= 4f;
        }

        //print(instrumentSources.Length);
        //print(baseSource.pitch);
        //if (MicrogameController.instance.isDebugMode() && Input.GetKeyDown(KeyCode.S))
        //    baseSource.pitch *= 4f;
        //if (MicrogameController.instance.isDebugMode() && Input.GetKeyUp(KeyCode.S))
        //    baseSource.pitch /= 4f;
        //print(baseSource.pitch);



        volumeLevels = volumeLevels.Select(a => 0f).ToArray();
	}

    public void SetVolumeLevel(Instrument instrument, float volume)
    {
        volumeLevels[(int)instrument] = Mathf.Max(volumeLevels[(int)instrument], volume);
    }
}
