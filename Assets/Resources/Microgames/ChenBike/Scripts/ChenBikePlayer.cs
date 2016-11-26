using UnityEngine;
using System.Collections;

public class ChenBikePlayer : MonoBehaviour
{
	public AudioClip BikeHorn;
	public Animator chenAnimator;
	public AudioSource honkSource;

	void Awake()
	{
		/* FEEDBACK: Do not use getComponent() every Update. It is slow.
		 * getComponent() should generally be stored as a value in Awake()
		 * 
		 * Or, as I like to do it, just make it a public variable and store it in the inspector, no need for GetComponent()
		 * plus then you can change the reference easily and without editing code
		 * */
		chenAnimator = GetComponent<Animator>();
		honkSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		//FEEDBACK: No need to nest two if statements here since you could just use an And (&&)
		//And Input.GetKeyDown("z") won't even be checked if the first part is false, meaning it's the same computation time
		if (chenAnimator.GetCurrentAnimatorStateInfo(1).IsName("HonkLayer.Idle") && Input.GetKeyDown("z"))
		{
			chenAnimator.Play("ChenHonk");
			//FEEDBACK: Use Time.timescale for pitch in audioSources, so the pitch is increased when the game is faster
			honkSource.pitch = Time.timeScale;
			honkSource.PlayOneShot(BikeHorn, 1F);
		}
	}
}
