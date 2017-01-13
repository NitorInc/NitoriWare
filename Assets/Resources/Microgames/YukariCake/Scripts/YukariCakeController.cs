using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukariCakeController : MonoBehaviour {

    // Properties
    public YukariCakeReimu Enemy;
    public List<AudioSource> AudioSources;
    public Animator YukariAnimator;
    public AudioSource YukariSource;
    public GameObject Food;

    // Temp
    public AudioClip Fail;

	// Use this for initialization
	void Start () {
        foreach (var a in AudioSources)
            a.pitch = Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () {
        if(!MicrogameController.instance.getVictoryDetermined())
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                YukariAnimator.SetTrigger("stateChange");
                
                if(Enemy.IsAlert)
                {
                    PlayFailureSound();
                    YukariAnimator.Play("YukariCakeYukariFail");
                    MicrogameController.instance.setVictory(false, true);
                }
                else
                {
                    PlayVictorySound();
                    YukariAnimator.Play("YukariCakeYukariVictory");
                    Destroy(Food);
                    MicrogameController.instance.setVictory(true, true);
                }
                Enemy.Stop();
            }
        }
	}

    public void PlayVictorySound()
    {
        // Better way of dealing with sounds
        Debug.Log("Woo!");
    }

    public void PlayFailureSound()
    {
        // Yeah
        YukariSource.PlayOneShot(Fail);
        Debug.Log(":(");
    }
}
