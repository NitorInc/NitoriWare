using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckVacuumParticles : MonoBehaviour {

    private bool suck;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //activates or deactivates a particle system

            if (suck == false)
            {
                ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
                suck = true;
            }
           
    }
}
