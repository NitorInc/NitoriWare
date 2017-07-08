using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextParentName : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Text text;
	
#pragma warning restore 0649

	void Start()
	{
        if (text == null)
            text = GetComponent<Text>();
        text.text = transform.parent.name;
    }
	
	void Update()
    {
        if (!Application.isPlaying)
            text.text = transform.parent.name;
    }
}
