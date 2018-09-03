using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashAnimatorSyncSpeed : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private float defaultBpm = 120f;
    
	void Start ()
    {
        var animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed *= timingData.Bpm / defaultBpm;
        }
	}
}
