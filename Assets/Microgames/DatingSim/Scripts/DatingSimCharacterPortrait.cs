using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimCharacterPortrait : MonoBehaviour {

    SpriteRenderer sr;
    
	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = DatingSimHelper.getSelectedCharacter().defaultPortait;
    }

    void onResult(bool victory)
    {
        var character = DatingSimHelper.getSelectedCharacter();
        sr.sprite = victory ? character.winPortrait : character.lossPortrait;
    }

    //[System.Serializable]
    //public class ListWrapper {
    //    public List<Sprite> list;
    //}
}
