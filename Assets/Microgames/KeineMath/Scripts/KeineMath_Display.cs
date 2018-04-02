using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Display : MonoBehaviour {

    private GameObject chalkboard;
    private GameObject displaytext;

	// Use this for initialization
	void Start () {
        chalkboard = GameObject.Find("Chalkboard");
        displaytext = this.transform.Find("DisplayText").gameObject;
        displaytext.GetComponent<TextMesh>().text = "0";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void processKeypress(string key)
    {

        if (key == "C") //Clear the display
        {
            displaytext.GetComponent<TextMesh>().text = "0";
            return;
        }

        if (key == "✓") //Submit the input answer for processing
        {
            chalkboard.GetComponent<KeineMath_Chalkboard>().processAnswer(displaytext.GetComponent<TextMesh>().text);
            return;
        }

        if (displaytext.GetComponent<TextMesh>().text.Length > 6) return; //Display is full

        if (displaytext.GetComponent<TextMesh>().text == "0") //First character: Overwrite the 0 with it
        {
            displaytext.GetComponent<TextMesh>().text = key;
            return;
        }

        //If none of the above: add the key onto the end of the display
        displaytext.GetComponent<TextMesh>().text += key;
    }
}
