using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPracticeMicrogameSpawner : MonoBehaviour
{
    private static float savedYPosition = 0;

    [SerializeField]
    private MenuPracticeMicrogame microgamePrefab;
    [SerializeField]
    private Transform parentObject;
    [SerializeField]
    private Vector3 topLeftPos;
    [SerializeField]
    private float separationDistance;
    [SerializeField]
    private int columnCount = 5;
    [SerializeField]
    private int zLevel = 0;
    [SerializeField]
    private RectTransform contentTransform;
    [SerializeField]
    private RectTransform collectionTransform;

    [Header("Passed down local references")]
    [SerializeField]
    private AudioSource buttonSfxSource;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text[] creditsTexts;
    [SerializeField]
    private GameMenu rootMenu;

    void Start()
    {
        var microgames = MicrogameHelper.getMicrogames(restriction: MicrogameTraits.Milestone.StageReady, includeBosses: false);
        int maxYIndex = 0;
        for (int i = 0; i < microgames.Count; i++)
        {
            var xIndex = i % columnCount;
            var yIndex = (i - (i % columnCount)) / columnCount;
            maxYIndex = yIndex;
            var xPos = topLeftPos.x + (xIndex * separationDistance);
            var yPos = topLeftPos.y - (yIndex * separationDistance);
            spawnPrefab(xPos, yPos, i);
        }

        var scrollAreaShift = separationDistance * maxYIndex;
        contentTransform.sizeDelta += Vector2.up * scrollAreaShift;
        collectionTransform.anchoredPosition += Vector2.up * scrollAreaShift / 2f;
        if (GameMenu.subMenu == GameMenu.SubMenu.Practice
            || GameMenu.subMenu == GameMenu.SubMenu.PracticeSelect)
            contentTransform.anchoredPosition += Vector2.up * savedYPosition;
    }

    private void Update()
    {
        savedYPosition = contentTransform.anchoredPosition.y;
    }

    void spawnPrefab(float x, float y, int index)
    {
        var newMicrogame = GameObject.Instantiate(microgamePrefab, parentObject);
        newMicrogame.GetComponent<MenuButton>().SfxSource = buttonSfxSource;
        newMicrogame.transform.SetSiblingIndex(index);
        newMicrogame.transform.localPosition = new Vector3(x, y, zLevel);
        newMicrogame.name = $"Microgame ({index.ToString()})";
        newMicrogame.NameText = nameText;
        newMicrogame.CreditsTexts = creditsTexts;
        newMicrogame.RootMenu = rootMenu;
    }
}