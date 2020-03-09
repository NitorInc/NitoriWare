using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButtonByShowcaseMode : MonoBehaviour
{
    [SerializeField]
    private MenuButton button;
    [SerializeField]
    private KeyCode buttonKey = KeyCode.S;
    
	void Start ()
    {
        if (GameController.instance.ShowcaseMode)
        {
            button.PressKey = buttonKey;
        }
	}
}
