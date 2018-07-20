using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoomGame_UI : MonoBehaviour
{
    [SerializeField]
    RectTransform hpBar, uhpBar, rArrow, lArrow;

    public static bool rightArrow, leftArrow;

    void Start()
    {
        rightArrow = leftArrow = false;
    }

    void Update()
    {
        hpBar.sizeDelta = new Vector2(uhpBar.sizeDelta.x * DoomGame_Player.hp / 100f, uhpBar.sizeDelta.y);
        rArrow.gameObject.SetActive(rightArrow);
        lArrow.gameObject.SetActive(leftArrow);
    }
}
