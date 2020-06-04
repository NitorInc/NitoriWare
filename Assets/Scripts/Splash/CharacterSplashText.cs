using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSplashText : MonoBehaviour
{

#pragma warning disable 0649
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

    private Phrase phrase;
    private float holdFadeDuration;

    void Start()
    {
        Cursor.visible = false;
        phrase = phrases[determinePhraseIndex()];
        setTexts(phrase);

        Invoke("queueShift", sceneTime / 2f);

        holdFadeDuration = GameController.instance.sceneShifter.getFadeDuration();
        GameController.instance.sceneShifter.setFadeDuration(1f);

    }

    void queueShift()
    {
        GameController.instance.sceneShifter.startShift(phrase.shiftScene, sceneTime / 2f, useFirstBuildIndex: true);
        GameController.instance.sceneShifter.setFadeDuration(holdFadeDuration);
    }

    int determinePhraseIndex()
    {
        return GameController.instance.ShowcaseMode ? 0 : (int)PrefsHelper.getProgress();
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
