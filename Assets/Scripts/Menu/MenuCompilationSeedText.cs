using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCompilationSeedText : MonoBehaviour
{
    InputField inputField;
    
	void Start ()
    {
        inputField = GetComponent<InputField>();
        inputField.text = CompilationStage.globalSeed;
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = "0";
        }
	}
	
	void Update ()
    {
        CompilationStage.globalSeed = inputField.text;
    }
}
