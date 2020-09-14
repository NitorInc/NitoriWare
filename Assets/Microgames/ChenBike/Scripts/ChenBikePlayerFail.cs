using UnityEngine;
using System.Collections;

public class ChenBikePlayerFail : MonoBehaviour
{
	public Animator chenAnimator;
    public bool dead = false;
    public bool audiocheck = true;
    public ChenCameraController chenspeed;
    public SpriteRenderer camera_darken;
    public SpriteRenderer alt_light;
    public SpriteRenderer light_system;
    public AudioSource honkSource;
    public AudioClip FailSound;
    public GameObject disablecounter;
    public GameObject chenshadowobject;
    public bool check_for_light;
    public bool isChenshadow;

	void Awake()
	{
		chenAnimator = GetComponent<Animator>();
        honkSource = GetComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update()
	{
        if (dead == true)
        {
            if (isChenshadow == true)
            {
                chenAnimator.Play("ChenShadowExit");
            }
            else
            {
                chenAnimator.Play("ChenRIP");
                honkSource.pitch = 1f;
                MicrogameController.instance.setVictory(false, true);
                chenspeed.speed = 1;
                Destroy(disablecounter);
                if (check_for_light == true)
                {
                    camera_darken.enabled = true;
                    alt_light.enabled = true;
                    light_system.enabled = false;

                }
                if (audiocheck == true)
                {
                    honkSource.PlayOneShot(FailSound);
                    audiocheck = false;
                }
            }
        }
    }
}
