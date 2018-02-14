using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimCharacters : MonoBehaviour
{
    [SerializeField]
    public List<Character> characters;

    [System.Serializable]
    public class Character
    {
        public string idName;
        public string fullName;
        [Multiline]
        public string introDialogue;
        public List<CharacterOption> rightOptions, wrongOptions;
        public AudioClip musicClip;
        public Sprite defaultPortait;
        public Sprite winPortrait;
        public Sprite lossPortrait;
        public Sprite backgroundImage;
        public Material optionBGMaterial;
    }

    [System.Serializable]
    public class CharacterOption
    {
        [Multiline]
        public string optionDialogue;
        [Multiline]
        public string responseDialogue;
        
        private bool correct;
        public void setCorrect(bool correct)
        {
            this.correct = correct;
        }
        public bool isCorrect()
        {
            return correct;
        }
    }
}
