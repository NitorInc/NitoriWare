using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandConductorNotePlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private BandConductorHandController handController;
    [SerializeField]
    private BandConductorNotePlayer passDownTo;
    [SerializeField]
    private AudioClip[] noteClips;
    [SerializeField]
    private AudioSource sfxSource;

    private Animator animator;
    private int notesPlayed;
    
	void Start ()
    {
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
		
	}

    public void PlayNote()
    {
        // Instantiate particles
        Instantiate(particlesPrefab, spawnPoint).transform.localPosition = Vector3.zero;
        animator.SetTrigger("Note");
        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
        sfxSource.PlayOneShot(noteClips[notesPlayed]);
        notesPlayed++;
        
        if (passDownTo != null && passDownTo != this)
            passDownTo.PlayNote();
        else
            handController.RegisterNote();
    }
}
