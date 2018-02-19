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
        public List<CharacterOption> rightOptions;
        public List<CharacterOption> wrongOptions;
        public AudioClip musicClip;
        public Sprite defaultPortait;
        public Sprite winPortrait;
        public Sprite lossPortrait;
        public Sprite backgroundImage;
        public Material optionBGMaterial;

        [Multiline]
        public string introDialogue;

        public string getLocalizedIntroDialogue()
        {
            return TextHelper.getLocalizedMicrogameText(idName + ".introdialogue", introDialogue);
        }

        public string getLocalizedDisplayName()
        {
            return TextHelper.getLocalizedMicrogameText(idName + ".fullname", fullName);
        }

        public string getLocalizedOptionDialogue(bool right, int index, bool response)
        {
            var option = right ? rightOptions[index] : wrongOptions[index];
            string optionKey = (right ? "right" : "wrong") + index.ToString();
            return TextHelper.getLocalizedMicrogameText(idName + "." + optionKey + "." + (response ? "response" : "option"),
                (response ? option.responseDialogue : option.optionDialogue));
        }
    }

    [System.Serializable]
    public class CharacterOption
    {
        [Multiline]
        public string optionDialogue;
        [Multiline]
        public string responseDialogue;
    }
}
