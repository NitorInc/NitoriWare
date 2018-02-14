using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimTraits : MicrogameTraits
{
    public DatingSimCharacters characterRoster;

    private DatingSimCharacters.Character selectedCharacter;

    public override AudioClip musicClip
    {
        get
        {
            return selectedCharacter.musicClip;
        }
    }

    public override void onAccessInStage(string microgameId)
    {
        selectedCharacter = characterRoster.characters[Random.Range(0, characterRoster.characters.Count)];
        
        base.onAccessInStage(microgameId);
    }
    
    public DatingSimCharacters.Character getSelectedCharacter()
    {
        return selectedCharacter;
    }
}
