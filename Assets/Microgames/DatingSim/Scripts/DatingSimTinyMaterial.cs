using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimTinyMaterial : MonoBehaviour {

    public Material[] mats;
	// Use this for initialization
	void Start () {
        //DialoguePreset.OnCharacterSelection += ChangeMaterial;
        //ChangeMaterial(DialoguePreset);
	}

    public void ChangeMaterial(int index) {
        GetComponent<Renderer>().material = new Material(mats[index]);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
