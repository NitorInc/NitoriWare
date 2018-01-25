using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDescriptionText : MonoBehaviour
{

#pragma warning disable 0649
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
    }
	
	void Update()
	{
        rectTransform.anchoredPosition += Vector2.left * xSpeed * Time.deltaTime;
        if (rectTransform.anchoredPosition.x < -(rectTransform.sizeDelta.x * transform.localScale.x) + minX)
            rectTransform.anchoredPosition = new Vector2(restartX, rectTransform.anchoredPosition.y);
    }

    public void activate(string text)
    {
        if (rectTransform == null)
            Start();
        if (GameMenu.shifting)
            return;
        rectTransform.anchoredPosition = new Vector2(startX, rectTransform.anchoredPosition.y);
        textComponent.text = text;
        baseObject.SetActive(true);
    }

    public bool isActivated()
    {
        return baseObject.activeInHierarchy;
    }

    public void deActivate()
    {
        baseObject.gameObject.SetActive(false);
    }
}
