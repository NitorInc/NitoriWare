using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckVacuumParticles : MonoBehaviour {

    private bool suck;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (suck == false)
            {
                ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
                suck = true;
            }
           
        }
        else
        {
            if (suck == true)
            {
                ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                particleSystem.Stop();
                suck = false;
            }
        }
    }
}
