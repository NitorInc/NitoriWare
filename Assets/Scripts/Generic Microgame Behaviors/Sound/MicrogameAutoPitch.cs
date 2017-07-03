using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Automatically adjusts pitch od all audiosources in gameobject, and its children if specified
public class MicrogameAutoPitch : MonoBehaviour
{
	[SerializeField]
	private bool includeChildren;
	private AudioSource[] sources;

	void Awake()
	{
		sources = includeChildren ? GetComponentsInChildren<AudioSource>() : GetComponents<AudioSource>();
	}

	void Start()
	{
		updatePitch();
	}

	public void updatePitch()
	{
		for (int i = 0; i < sources.Length; i++)
		{
			sources[i].pitch = Time.timeScale;
		}
	}
}
