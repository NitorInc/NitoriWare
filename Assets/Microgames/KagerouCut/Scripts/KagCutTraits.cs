﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/KagerouCut/Traits")]
public class KagCutTraits : MicrogameTraits {
    [SerializeField]
    public KagerouCutController.Dog[] dogs;
    [HideInInspector]
    public KagerouCutController.Dog dog;

    [HideInInspector]
    private AudioClip _randomMusicClip;
    public override AudioClip musicClip => _randomMusicClip;
    
    //private AudioClip _musicClip;
	
    public override void onAccessInStage(string microgameId, int difficulty) {
		base.onAccessInStage(microgameId, difficulty);
		dog = dogs[Random.Range(0, dogs.Length)];
        _randomMusicClip = dog.bgm;
	}
}
