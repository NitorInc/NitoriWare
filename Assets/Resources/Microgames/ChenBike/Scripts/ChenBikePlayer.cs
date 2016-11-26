using UnityEngine;
using System.Collections;

public class ChenBikePlayer : MonoBehaviour {
    public AudioClip BikeHorn;

    // Use this for initialization
    void Start () {

    }
	// Update is called once per frame
	void Update () {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("HonkLayer.Idle"))
        {
            if (Input.GetKeyDown("z"))
            {
                GetComponent<Animator>().Play("ChenHonk");
                GetComponent<AudioSource>().PlayOneShot(BikeHorn, 1F);
            }
        }
	}
}
