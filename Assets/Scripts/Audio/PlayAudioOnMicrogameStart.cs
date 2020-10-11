using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnMicrogameStart : MonoBehaviour
{

	void Start()
	{
        AudioHelper.playScheduled(GetComponent<AudioSource>(), (float)Microgame.BeatLength);
	}
	
	void play()
	{
	}
}
