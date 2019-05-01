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
        Invoke("newRap", startBeat * StageController.beatLength);
	}

    void advanceTextBox()
    {

    }

    void newRap()
    {
        var rap = raps[rapIndex];
        var newLine = Instantiate(textPrefab).GetComponent<RapBattleTextAnimation>();
        newLine.setRap(rap);
        newLine.transform.position = textSpawnAnchor.position + (Vector3.down * textYSeparation * rapIndex);

        marisaAnimator.SetBool("Rapping", true);

        rapIndex++;
        if (rapIndex < raps.Length)
            Invoke("newRap", beatsPerRap * StageController.beatLength);
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
