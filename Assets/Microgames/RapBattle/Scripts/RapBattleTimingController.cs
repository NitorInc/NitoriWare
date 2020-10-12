using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RapBattleTimingController : MonoBehaviour
{
    [SerializeField]
    private RapBattleRapData rapData;

    [SerializeField]
    private int lineCount = 2;
    [SerializeField]
    private string forceRap;
    [SerializeField]
    private float startBeat = 3f;
    [SerializeField]
    private float beatsPerRap = 2f;
    [SerializeField]
    private float finalRapBeats = 1f;
    [SerializeField]
    private GameObject textPrefab;
    [SerializeField]
    private Transform textSpawnAnchor;
    [SerializeField]
    private float textYSeparation;
    [SerializeField]
    private float textBoxAppearPreTime;
    [SerializeField]
    private RapBattleChoice[] choiceBoxes;

    [SerializeField]
    private Animator marisaAnimator;
    [SerializeField]
    private Animator speechBubbleAnimator;

    int lineIndex = 0;
    RapBattleRapData.Rap rap;
    int rapIndex;

	void Awake()
    {
        var rapPool = rapData.Raps.Where(a => a.Lines.Length == lineCount).ToArray();

        if (!string.IsNullOrEmpty(forceRap))
            rapPool = rapPool.Where(a => a.Name.Equals(forceRap, System.StringComparison.OrdinalIgnoreCase)).ToArray();

        rapIndex = Random.Range(0, rapPool.Length);
        rap = rapPool[rapIndex];

        setChoiceData();
        scheduleRap(startBeat);
    }

    void setChoiceData()
    {
        //TODO localize

        var choicePool = choiceBoxes.ToList();
        choicePool.Shuffle();
        var wrongPool = rap.WrongAnswers.ToList();
        wrongPool.Shuffle();

        choicePool[0].setData(rap.Answer, rapData.RhymeHighlightColor, true);
        for (int i = 1; i < choicePool.Count(); i++)
        {
            choicePool[i].setData(wrongPool[i], rapData.RhymeHighlightColor, false);
        }
    }

    void scheduleRap(float beats)
    {
        Invoke("newRap", beats * (float)Microgame.BeatLength);
        Invoke("advanceTextBox", (beats * (float)Microgame.BeatLength) - textBoxAppearPreTime);
    }

    void advanceTextBox()
    {
        speechBubbleAnimator.SetInteger("Stage", lineIndex);
    }

    void newRap()
    {
        var line = rap.Lines[lineIndex];

        var newLineObject = Instantiate(textPrefab).GetComponent<RapBattleTextAnimation>();
        //TODO localize
        newLineObject.setData(line.Verse, line.Rhyme, rapData.RhymeHighlightColor);

        var holdScale = newLineObject.transform.localScale;
        newLineObject.transform.SetParent(textSpawnAnchor);
        newLineObject.transform.localPosition = Vector3.down * textYSeparation * lineIndex;
        newLineObject.transform.localScale = holdScale;

        marisaAnimator.SetBool("Rapping", true);

        lineIndex++;
        if (lineIndex < rap.Lines.Length)
            scheduleRap(beatsPerRap);
        else
            Invoke("end", finalRapBeats * (float)Microgame.BeatLength);
    }

    void end()
    {
        marisaAnimator.SetBool("Rapping", false);
        speechBubbleAnimator.SetTrigger("Choose");
    }
}
