using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingMusic : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private float loopStartTime, loopEndTime;
#pragma warning restore 0649

    private AudioSource _audioSource;

	void Awake()
	{
        _audioSource = GetComponent<AudioSource>();
	}
	
	void Update()
	{
        if (_audioSource.time >= loopEndTime)
            _audioSource.time -= loopEndTime - loopStartTime;
	}
}
