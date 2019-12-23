using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPunchTarget : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float punchScreenshake = .1f;
    [SerializeField]
    private AudioClip hitSound;
    [SerializeField]
    private int targetsRequired = 1;
    [SerializeField]
    private float secondHitPitch = 1.1f;
    [SerializeField]
    private AudioClip victoryClip;
    [SerializeField]
    private float victorySoundDelay = .25f;

    private static int targetsHit;
    
	void Start ()
    {
        targetsHit = 0;
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.tag.Equals("MicrogameTag2"))
        {
            enabled = false;
            rigAnimator.SetTrigger("Hit");
            CameraShake.instance.addScreenShake(punchScreenshake);
            targetsHit++;
            if (targetsHit >= targetsRequired)
            {
                MicrogameController.instance.setVictory(true);
                AudioHelper.playScheduled(GetComponent<AudioSource>(), victorySoundDelay);
            }

            var sfxSource = GetComponent<AudioSource>();
            sfxSource.PlayOneShot(hitSound);
            sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
            if (targetsHit > 1)
                sfxSource.pitch *= secondHitPitch;
        }
    }
    
}
