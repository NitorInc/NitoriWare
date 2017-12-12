using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_CursorScript : MonoBehaviour {

    public GameObject hitbox;
    public static Animator anim;

    [SerializeField]
    private AudioClip hitSound;

    private bool swinging = false;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnMouseDown()
    {
        if (swinging == false)
        {
            swinging = true;
            Invoke("createHitbox", 0.09f);
            Invoke("swingReady", 0.18f);
            MicrogameController.instance.playSFX(hitSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(0));
            anim.SetBool("Swing", true);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    void createHitbox()
    {
        GameObject myHitbox = Instantiate(hitbox);
        myHitbox.transform.position = transform.position;
    }

    void swingReady()
    {
        swinging = false;
        anim.SetBool("Swing", false);
    }
}
