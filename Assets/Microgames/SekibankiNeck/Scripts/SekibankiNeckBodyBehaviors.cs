using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekibankiNeckBodyBehaviors : MonoBehaviour
{
    [SerializeField]
    private AudioClip goalSound;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite victorypose;
    private Animator animator;



        
            
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.gameObject.tag == "MicrogameTag3")
        {
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();

            MicrogameController.instance.playSFX(goalSound, volume: 1f, panStereo: AudioHelper.getAudioPan(transform.position.x));
            Animator animator = GetComponentInChildren<Animator>();
            animator.enabled = (false);
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = victorypose;



        }


    }
}
