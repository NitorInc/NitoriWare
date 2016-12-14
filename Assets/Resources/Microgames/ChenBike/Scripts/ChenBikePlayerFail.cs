using UnityEngine;
using System.Collections;

public class ChenBikePlayerFail : MonoBehaviour
{
	public Animator chenAnimator;
    public bool dead = false;
    public ChenCameraController chenspeed;
    public SpriteRenderer camera_darken;
    public SpriteRenderer alt_light;
    public SpriteRenderer light_system;
    public bool check_for_light;

	void Awake()
	{
		chenAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
        if (dead == true)
        {
            chenAnimator.Play("ChenRIP");
            MicrogameController.instance.setVictory(false, true);
            chenspeed.speed = 1;
            if (check_for_light == true)
            {
                camera_darken.enabled = true;
                alt_light.enabled = true;
                light_system.enabled = false;
            }
        }
    }
}
