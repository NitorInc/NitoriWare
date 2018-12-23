using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashOnBeatAnimation : MonoBehaviour
{
    [SerializeField]
    private string triggerName = "Beat";

    Animator beatAnimator;

	void Start()
    {
        YoumuSlashTimingController.onBeat += onBeat;
        beatAnimator = GetComponent<Animator>();
	}

    void onBeat(int beat)
    {
        beatAnimator.SetTrigger(triggerName);
    }
}
