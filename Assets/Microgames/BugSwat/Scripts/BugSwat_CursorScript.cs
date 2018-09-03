using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_CursorScript : MonoBehaviour {

    public GameObject hitbox;
    public GameObject dust;
    public static Animator anim;

    [SerializeField]
    private AudioClip hitSound;

    private bool swinging = false;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && swinging == false)
        {
            swinging = true;
            Invoke("createHitbox", 0.09f);
            Invoke("swingReady", 0.3f);
            MicrogameController.instance.playSFX(hitSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(0));
            anim.SetBool("Swing", true);
        }
		
	}

    void createHitbox()
    {
        GameObject myHitbox = Instantiate(hitbox);
        myHitbox.transform.position = transform.position;
        GameObject myDust = Instantiate(dust);
        myDust.transform.position = new Vector3(transform.position.x, transform.position.y, myDust.transform.position.z);
    }

    void swingReady()
    {
        swinging = false;
        anim.SetBool("Swing", false);
    }
}
