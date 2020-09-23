using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class VoicePlayer : MonoBehaviour
{
	[SerializeField]
	private AudioSource voiceSource;

	private List<AudioClip> victoryClips, lossClips;
	private AudioClip lastClip;
	private List<VoiceSession> activeSessions;

	class VoiceSession
    {
        public Microgame.Session MicrogameSession { get; private set; }
        public bool ClipPlayed { get; set; }
		public VoiceSession(Microgame.Session microgameSession)
		{
			MicrogameSession = microgameSession;
			ClipPlayed = false;
		}
	}

	public enum VoiceSet
	{
		None,
		Nitori
	}

    private void Awake()
    {
		activeSessions = new List<VoiceSession>();
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

	public void PlayClip(Microgame.Session session)
	{
		var voiceSession = new VoiceSession(session);
		activeSessions.Add(voiceSession);
		StartCoroutine(PlayWithDelay(voiceSession));

	}

	IEnumerator PlayWithDelay(VoiceSession voiceSession)
    {
		yield return new WaitForSeconds(voiceSession.MicrogameSession.GetVoiceDelay());
		if (!voiceSession.ClipPlayed)
        {
			voiceSession.ClipPlayed = true;
			playClipAudio(voiceSession.MicrogameSession.VictoryStatus);
        }
    }

	/// <summary>
	/// Force play if play is scheduled, used when microgame ends
	/// </summary>
	public void ForcePlay(Microgame.Session session)
	{
		// Check if audio is already scheduled to play or has played
		var voiceSession = activeSessions.FirstOrDefault(a => a.MicrogameSession == session);
		if (voiceSession != null)
		{
			if (!voiceSession.ClipPlayed)
            {
				voiceSession.ClipPlayed = true;
				playClipAudio(session.VictoryStatus);
            }
			activeSessions.Remove(voiceSession);
		}
		else
			playClipAudio(session.VictoryStatus);
	}

	void playClipAudio(bool victory)
	{
		if ((victory && victoryClips.Count == 0) || (!victory && lossClips.Count == 0))
			return;

		voiceSource.pitch = Mathf.Pow(Time.timeScale, .5f);
		List<AudioClip> clipPool = victory ? victoryClips : lossClips;

		AudioClip clipToPlay;
		do
		{
			clipToPlay = clipPool[Random.Range(0, clipPool.Count)];
		}
		while (clipToPlay == lastClip || clipPool.Count < 2);

		voiceSource.PlayOneShot(clipToPlay);
		lastClip = clipToPlay;
	}

	public void StopPlayback()
	{
		CancelInvoke();
		voiceSource.Stop();
    }
}
