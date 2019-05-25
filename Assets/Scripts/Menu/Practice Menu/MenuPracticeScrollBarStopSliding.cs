using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPracticeScrollBarStopSliding : MonoBehaviour
{

    private ScrollRect scrollRect;
    
	void Start ()
    {
        scrollRect = GetComponent<ScrollRect>();
	}

    public void onValueChanged(Vector2 value)
    {
        if (Input.mouseScrollDelta != Vector2.zero)
            scrollRect.velocity = Vector2.zero;
    }
}
