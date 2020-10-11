using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeStageButtons : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Microgame.Milestone restriction;
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private Vector2 topLeftPosition;
    [SerializeField]
    private float xSeparation, ySeparation;
    [SerializeField]
    private int buttonsPerRow;
#pragma warning restore 0649

	void Start()
	{
        var microgames = MicrogameHelper.getMicrogames(restriction);
		for (int i = 0; i < microgames.Count; i++)
        {
            int column = i % buttonsPerRow, row = (i - column) / buttonsPerRow;
            Vector2 position = topLeftPosition + new Vector2(column * xSeparation, row * -ySeparation);

            RectTransform rectTransform = Instantiate(buttonPrefab, transform).GetComponent<RectTransform>();
            rectTransform.anchoredPosition = position;
            rectTransform.name = microgames[i].microgameId;
        }
	}
}
