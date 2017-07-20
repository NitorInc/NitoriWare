using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSplashText : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Text[] lineTexts;
    [SerializeField]
    private Phrase[] phrases;
    [SerializeField]
    private float sceneTime;
#pragma warning restore 0649

    [System.Serializable]
    public struct Phrase
    {
        public string key;
        [Multiline]
        public string defaultValue;
        public LocalizedText.Parameter[] parameters;
        public string shiftScene;
    }

    void Start()
    {
        Cursor.visible = false;
        Phrase phrase = phrases[determinePhraseIndex()];
        setTexts(phrase);
        GameController.instance.sceneShifter.startShift(phrase.shiftScene, sceneTime);
    }

    int determinePhraseIndex()
    {
        int progress = PrefsHelper.getProgress();
        return progress < 2 ? progress : 2;
    }

    void setTexts(Phrase phrase)
    {
        string[] lines = TextHelper.getLocalizedText(phrase.key, phrase.defaultValue, phrase.parameters).Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lineTexts[i].text = lines[i < lines.Length ? i : 0];
        }
    }
}
