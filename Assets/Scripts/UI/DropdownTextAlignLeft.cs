using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DropdownTextAlignLeft : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private bool onlyUpdateOnChangeText = true;
    [SerializeField]
    private float initialX, initialXSize;
#pragma warning restore 0649

    private string lastUpdatedText;

	void Start()
	{
        textComponent = GetComponent<Text>();
        lastUpdatedText = textComponent.text;
        rectTransform = (RectTransform)transform;
        initialX = rectTransform.position.x;
        initialXSize = rectTransform.sizeDelta.x;
	}
	
	void Update()
	{
		if ((!onlyUpdateOnChangeText || lastUpdatedText != textComponent.text))
            updateSize();
	}

    void updateSize()
    {
        rectTransform.position = new Vector3(initialX + (rectTransform.sizeDelta.x - initialXSize), rectTransform.position.y, rectTransform.position.z);
        lastUpdatedText = textComponent.text;
    }
}
