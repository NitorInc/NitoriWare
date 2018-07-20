using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextParentName : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Text text;
    [SerializeField]
    private bool editorModeOnly = true;
	
#pragma warning restore 0649

	void Start()
	{
        if (text == null)
            text = GetComponent<Text>();

        if (!editorModeOnly || !Application.isPlaying)
            text.text = transform.parent.name;
    }
	
	void Update()
    {
        if (!Application.isPlaying)
            text.text = transform.parent.name;
    }
}
