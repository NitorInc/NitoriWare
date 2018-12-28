using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashMyonAnimator : MonoBehaviour
{
    [SerializeField]
    private string triggerName = "Beat";
    [SerializeField]
    private YoumuSlashPlayerController player;
    [SerializeField]
    private bool onlyPlayBeatIfYoumuBobs;

    Animator beatAnimator;

	void Start()
    {
        YoumuSlashTimingController.onBeat += onBeat;
        beatAnimator = GetComponent<Animator>();
	}

    void onBeat(int beat)
    {
        if (!onlyPlayBeatIfYoumuBobs || player.getBobEnabled())
            beatAnimator.SetTrigger(triggerName);
    }
}
