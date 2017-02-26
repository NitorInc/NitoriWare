using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePlayer : MonoBehaviour
{
	[SerializeField]
	private AudioSource voiceSource;

	private List<AudioClip> victoryClips, lossClips;
	private bool soundQueued, victory;

	public enum VoiceSet
	{
		None,
		Nitori
	}


	void Start()
	{
		soundQueued = false;
	}

	/// <summary>
	/// Loads clips and initiates character
	/// </summary>
	public void loadClips(VoiceSet voiceSet)
	{
		victoryClips = new List<AudioClip>();
		lossClips = new List<AudioClip>();
		if (voiceSet == VoiceSet.None)
			return;

		string set = voiceSet.ToString();
		int i = 0;
		AudioClip newClip;

		while (true)
		{
			newClip = Resources.Load<AudioClip>("Voices/" + set + "/Victory" + i.ToString());
			if (newClip == null)
				break;
			else
				victoryClips.Add(newClip);
			i++;
		}

		i = 0;
		while (true)
		{
			newClip = Resources.Load<AudioClip>("Voices/" + set + "/Loss" + i.ToString());
			if (newClip == null)
				break;
			else
				lossClips.Add(newClip);
			i++;
		}
	}

	/// <summary>
	/// Plays or schedules a voice clip
	/// </summary>
	/// <param name="victory"></param>
	/// <param name="delayTime"></param>
	public void playClip(bool victory, float delayTime)
	{
		soundQueued = false;
		this.victory = victory;

		//voiceSource.clip = victory ? victoryClips[Random.Range(0, victoryClips.Count)]
		//	: lossClips[Random.Range(0, lossClips.Count)];
		if (delayTime == 0f)
			playClipAudio();
		else
		{
			soundQueued = true;
			Invoke("playClipAudio", delayTime);
		}
	}

	/// <summary>
	/// Force play if play is scheduled, used when microgame ends
	/// </summary>
	public void forcePlay()
	{
		if (soundQueued)
		{
			CancelInvoke();
			playClipAudio();
		}
	}

	void playClipAudio()
	{
		if ((victory && victoryClips.Count == 0) || (!victory && lossClips.Count == 0))
			return;

		voiceSource.pitch = Time.timeScale;
		voiceSource.PlayOneShot(victory ? victoryClips[Random.Range(0, victoryClips.Count)]
			: lossClips[Random.Range(0, lossClips.Count)]);
		soundQueued = false;
	}
}
