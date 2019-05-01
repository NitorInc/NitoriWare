using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapBattleTimingController : MonoBehaviour
{
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
    private Animator marisaAnimator;
    [SerializeField]
    private Animator speechBubbleAnimator;

    //Placeholder
    public Rap[] raps;
    [System.Serializable]
    public class Rap
    {
        public string verse;
        public string rhyme;
        public string highlightColor;
    }

    int rapIndex = 0;

	void Start ()
    {
        scheduleRap(startBeat);
    }

    void scheduleRap(float beats)
    {
        Invoke("newRap", beats * StageController.beatLength);
        Invoke("advanceTextBox", (beats * StageController.beatLength) - textBoxAppearPreTime);
    }

    void advanceTextBox()
    {
        speechBubbleAnimator.SetInteger("Stage", rapIndex);
    }

    void newRap()
    {
        var rap = raps[rapIndex];
        var newLine = Instantiate(textPrefab).GetComponent<RapBattleTextAnimation>();
        newLine.setRap(rap);

        var holdScale = newLine.transform.localScale;
        newLine.transform.parent = textSpawnAnchor;
        newLine.transform.localPosition = Vector3.down * textYSeparation * rapIndex;
        newLine.transform.localScale = holdScale;

        marisaAnimator.SetBool("Rapping", true);

        rapIndex++;
        if (rapIndex < raps.Length)
            scheduleRap(beatsPerRap);
        else
            Invoke("end", finalRapBeats * StageController.beatLength);
    }

    void end()
    {
        marisaAnimator.SetBool("Rapping", false);
    }
	
	void Update ()
    {
		
	}
}
