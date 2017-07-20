using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MenuCreditsPlacement : MonoBehaviour
{
    
#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private int distanceBetweenCells, distancePerCreditLine;
#pragma warning restore 0649

	void Start()
	{
		if (Application.isPlaying)
        {
            enabled = false;
            return;
        }
	}
	
	void Update()
	{
        int currentY = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform cell = (RectTransform)transform.GetChild(i);
            cell.anchoredPosition = Vector2.down * currentY;

            currentY += distanceBetweenCells;
            currentY += ((getLineCount(cell.GetChild(1)) - 1) * distancePerCreditLine);
        }
	}

    int getLineCount(Transform transform)
    {
        return transform.GetComponent<Text>().text.Split('\n').Length;
    }
}
