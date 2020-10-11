using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/DatingSim/Option Data")]
public class DatingSimOptionData : ScriptableObject
{
    public string charId;
    public Option[] rightOptions;
    public Option[] wrongOptions;

    [System.Serializable]
    public class Option
    {
        [Multiline]
        public string optionDialogue;
        [Multiline]
        public string responseDialogue;
    }

}
