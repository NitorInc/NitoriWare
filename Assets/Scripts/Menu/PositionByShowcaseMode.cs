using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionByShowcaseMode : MonoBehaviour
{
    [SerializeField]
    private Vector3 showcaseModePosition;
    [SerializeField]
    private bool rect = false;
    
	void Start ()
    {
		if (GameController.instance.ShowcaseMode)
        {
            if (rect)
            {
                var rectT = GetComponent<RectTransform>();
                rectT.anchoredPosition = showcaseModePosition;
            }
            else
                transform.localPosition = showcaseModePosition;
        }
	}
}
