using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/DatingSim/Traits")]
public class DatingSimTraits : MicrogameTraits
{
    public DatingSimCharacters characterRoster;
    public int overrideCharacter = -1;

    public override AudioClip GetMusicClip(MicrogameSession session) => ((DatingSimSession)session).character.musicClip;

    public override MicrogameSession onAccessInStage(string microgameId, int difficulty)
    {
        DatingSimCharacters.Character selectedCharacter;

        if (overrideCharacter > -1)
            selectedCharacter = characterRoster.characters[overrideCharacter];
        else
            selectedCharacter = characterRoster.characters[Random.Range(0, characterRoster.characters.Count)];
        
        return new DatingSimSession(microgameId, difficulty, selectedCharacter);
    }
    
    public DatingSimCharacters.Character getSelectedCharacter(DatingSimSession session)
    {
        return session.character;
    }
}
