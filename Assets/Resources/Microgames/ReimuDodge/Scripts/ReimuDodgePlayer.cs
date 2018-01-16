using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgePlayer : MonoBehaviour
{

    [SerializeField]
    private AudioClip deathSound;
    private bool alive = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
            Kill();
    }
    void Kill()
    {
        MicrogameController.instance.setVictory(false, true);
        alive = false;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        GetComponent<FollowCursor>().enabled = false;
        MicrogameController.instance.playSFX(deathSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
