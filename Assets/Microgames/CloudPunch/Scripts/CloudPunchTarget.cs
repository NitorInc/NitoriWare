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
    
	void Start ()
    {
		
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
            var sfxSource = GetComponent<AudioSource>();
            sfxSource.PlayOneShot(hitSound);
            sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
        }
    }
}
