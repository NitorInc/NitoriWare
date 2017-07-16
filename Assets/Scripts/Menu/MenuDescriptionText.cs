using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionText : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private GameObject baseObject;
    [SerializeField]
    private Text textComponent;
    [SerializeField]
    private float startX, xSpeed, minX, restartX;
    private int test;
#pragma warning restore 0649

    private RectTransform rectTransform;

	void Start()
	{
        rectTransform = (RectTransform)transform;
        baseObject.SetActive(false);
        activate(TextHelper.getLocalizedText("menu.gamemode.compilation.description", "Oh no i can find it"));
    }

    public void activate(string text)
    {
        rectTransform.anchoredPosition = new Vector2(startX, rectTransform.anchoredPosition.y);
        textComponent.text = text;
        baseObject.SetActive(true);
    }
	
	void Update()
	{
        rectTransform.anchoredPosition += Vector2.left * xSpeed * Time.deltaTime;
        if (rectTransform.anchoredPosition.x < -(rectTransform.sizeDelta.x * transform.localScale.x) + minX)
            rectTransform.anchoredPosition = new Vector2(restartX, rectTransform.anchoredPosition.y);
	}
}
