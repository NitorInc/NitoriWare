using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YoumuSlashDebugText : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    [SerializeField]
    private float beatsActive = 2f;
    public float BeatsActive
    {
        get { return beatsActive; }
        set { beatsActive = value; }
    }

    private TextMesh textComponent;


    void Start ()
    {
        textComponent = GetComponent<TextMesh>();
	}
	
	public void setText (string text)
    {
        textComponent.text = text;
        CancelInvoke();
        Invoke("resetText", beatsActive * timingData.BeatDuration);
	}

    void resetText()
    {
        textComponent.text = "";
    }
}
