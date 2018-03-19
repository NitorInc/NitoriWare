using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

public class NueAbductController : MonoBehaviour
{
    [Header("All animals that need to be sucked up")]
    [SerializeField]
    private GameObject[] animals;

	// Check victory condition (all animals sucked)
	void Update () {
	    foreach (GameObject animal in animals)
	        if (animal)
	            return;
	    // No animals left, we win
		MicrogameController.instance.setVictory(victory: true, final: true);
	}
}
