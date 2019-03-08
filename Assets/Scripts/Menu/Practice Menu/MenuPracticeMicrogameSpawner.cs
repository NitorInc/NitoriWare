using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    [SerializeField]
    private RectTransform scrollViewTransform;
    [SerializeField]
    private Scrollbar verticalScrollbar;

    [Header("Passed down local references")]
    [SerializeField]
    private AudioSource buttonSfxSource;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text[] creditsTexts;
    [SerializeField]
    private GameMenu rootMenu;

    public static List<MicrogameCollection.Microgame> standardMicrogamePool;
    public static List<MicrogameCollection.Microgame> microgameBossPool;

    void Start()
    {
        // Create microgames
        standardMicrogamePool = MicrogameHelper.getMicrogames(restriction: MicrogameTraits.Milestone.StageReady);
        microgameBossPool = MicrogameHelper.getMicrogames(restriction: MicrogameTraits.Milestone.StageReady, includeBosses: true)
            .Where(a => a.difficultyTraits[0].isBossMicrogame()).ToList();
        int maxYIndex = 0;

        var standardCount = standardMicrogamePool.Count;
        var totalCount = standardCount + microgameBossPool.Count();
        for (int i = 0; i < totalCount; i++)
        {
            bool isBoss = i >= standardCount;

            var xIndex = i % columnCount;
            var yIndex = (i - (i % columnCount)) / columnCount;
            maxYIndex = yIndex;
            var xPos = topLeftPos.x + (xIndex * separationDistance);
            var yPos = topLeftPos.y - (yIndex * separationDistance);
            spawnPrefab(xPos, yPos, 
                isBoss ? i - standardCount : i,
                isBoss);
        }


        // Expand scroll area and cheat some scroll view values
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

    void spawnPrefab(float x, float y, int index, bool isBoss)
    {
        var newMicrogame = GameObject.Instantiate(microgamePrefab, parentObject);
        newMicrogame.GetComponent<MenuButton>().SfxSource = buttonSfxSource;
        newMicrogame.transform.SetSiblingIndex(index);
        newMicrogame.transform.localPosition = new Vector3(x, y, zLevel);
        newMicrogame.name = !isBoss ?
            $"Microgame ({index.ToString()})"
            : $"Boss ({index.ToString()})";
        newMicrogame.NameText = nameText;
        newMicrogame.CreditsTexts = creditsTexts;
        newMicrogame.RootMenu = rootMenu;
    }
}