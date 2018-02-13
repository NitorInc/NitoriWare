using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    SpriteRenderer sr;
    public Sprite[] bgSprites;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        DialoguePreset.OnCharacterSelection += SetBackground;
	}

    void SetBackground(int index) {
        sr.sprite = bgSprites[index];
    }

}
