using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimBackgroundController : MonoBehaviour {

    SpriteRenderer sr;
	// Use this for initialization
	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        //sr.sprite = DatingSimHelper.getSelectedCharacter().backgroundImage;
	}

}
