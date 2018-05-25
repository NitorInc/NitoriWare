using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/DatingSim/Traits")]
public class DatingSimTraits : MicrogameTraits
{
    public DatingSimCharacters characterRoster;
    public int overrideCharacter = -1;

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
        if (overrideCharacter > -1)
            selectedCharacter = characterRoster.characters[overrideCharacter];
        else
            selectedCharacter = characterRoster.characters[Random.Range(0, characterRoster.characters.Count)];
        
        base.onAccessInStage(microgameId);
    }
    
    public DatingSimCharacters.Character getSelectedCharacter()
    {
        return selectedCharacter;
    }
}
