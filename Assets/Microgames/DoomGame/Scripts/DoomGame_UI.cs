using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoomGame_UI : MonoBehaviour
{
    [SerializeField]
    RectTransform rArrow, lArrow;

    public static bool rightArrow, leftArrow;

    void Start()
    {
        rightArrow = leftArrow = false;
    }

    void Update()
    {
        if(rArrow.gameObject.activeSelf != rightArrow)
            rArrow.gameObject.SetActive(rightArrow);
        if(lArrow.gameObject.activeSelf != leftArrow)
            lArrow.gameObject.SetActive(leftArrow);
    }
}
