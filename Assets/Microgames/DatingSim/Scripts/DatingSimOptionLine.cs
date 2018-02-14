using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DatingSimOptionLine : MonoBehaviour {

    TMP_Text textComp;
    DatingSimCursor cursor;
    public Color defaultColor;
    public Color greyColor;

	// Use this for initialization
	void Start () {

	}

    public void SetText(string text) {
        if (textComp == null)
            textComp = GetComponentInChildren<TMP_Text>();
        if (cursor == null) {
            cursor = GetComponentInChildren<DatingSimCursor>();
            cursor.gameObject.SetActive(false);
        }
        
        textComp.text = text;
    }

    public void HighlightText(bool highlight) {
        if (highlight)
            textComp.color = defaultColor;
        else
            textComp.color = greyColor;
    }

    public void ShowText(bool show) {
        if (show) {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void ShowCursor(bool show) {
        if (show) {
            cursor.gameObject.SetActive(true);
            HighlightText(true);
        }
        else {
            cursor.gameObject.SetActive(false);
            HighlightText(false);
        }
    }
}
