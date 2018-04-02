using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineMath_Key : MonoBehaviour {

    //✓
    [Header("Which key this is")]
    [SerializeField]
    private string symbol = "E";

    private GameObject display;
    private GameObject keytext;

    // Use this for initialization
    void Start () {
        display = GameObject.Find("Display");
        keytext = this.transform.Find("KeyText").gameObject;
        keytext.GetComponent<TextMesh>().text = symbol;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        display.GetComponent<KeineMath_Display>().processKeypress(symbol);
    }
}
