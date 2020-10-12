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
    private float bossSeparation = 30f;
    [SerializeField]
    private int zLevel = 0;
    [SerializeField]
    private float disableYThreshold = 5f;
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

    private List<MenuPracticeMicrogame> spawnedMicrogames;
    private Sprite[] microgameIcons;

    public static List<Microgame> standardMicrogamePool;
    public static List<Microgame> microgameBossPool;

    void Start()
    {
        // Create microgames

        var microgamePool = MicrogameHelper.getMicrogames(restriction: Microgame.Milestone.StageReady, includeBosses: true);

        standardMicrogamePool = microgamePool
            .Where(a => !a.isBossMicrogame())
            .ToList();
        microgameBossPool = microgamePool
            .Where(a => a.isBossMicrogame())
            .ToList();

        int maxYIndex = 0;
        spawnedMicrogames = new List<MenuPracticeMicrogame>();

        microgameIcons = Resources.LoadAll<Sprite>("MicrogameIcons");

        var standardCount = standardMicrogamePool.Count;
        var totalCount = standardCount + microgameBossPool.Count();
        for (int i = 0; i < totalCount; i++)
        {
            bool isBoss = i >= standardCount;
            var xIndex = i % columnCount;
            var yIndex = (i - (i % columnCount)) / columnCount;
            if (isBoss)
            {
                var bossIndex = i - standardCount;
                yIndex = (((standardCount-1) - ((standardCount-1) % columnCount)) / columnCount) + 1;   // Set to one row below last standard microgame
                yIndex += ((bossIndex - (bossIndex % columnCount)) / columnCount);  // Plus however many rows down it is in bosses
                xIndex = bossIndex % columnCount;
            }

            maxYIndex = yIndex;
            var xPos = topLeftPos.x + (xIndex * separationDistance);
            var yPos = topLeftPos.y - (yIndex * separationDistance);
            if (isBoss)
                yPos -= bossSeparation;
            spawnPrefab(xPos, yPos, 
                isBoss ? i - standardCount : i,
                isBoss);
        }


        // Expand scroll area and cheat some scroll view values
        var scrollAreaShift = separationDistance * maxYIndex;
        scrollAreaShift += bossSeparation;
        contentTransform.sizeDelta += Vector2.up * scrollAreaShift;
        collectionTransform.anchoredPosition += Vector2.up * scrollAreaShift / 2f;
        if (GameMenu.subMenu == GameMenu.SubMenu.Practice
            || GameMenu.subMenu == GameMenu.SubMenu.PracticeSelect)
            contentTransform.anchoredPosition += Vector2.up * savedYPosition;
    }

    private void Update()
    {
        savedYPosition = contentTransform.anchoredPosition.y;
        foreach (var microgame in spawnedMicrogames)
        {
            bool isActive = microgame.gameObject.activeInHierarchy;
            bool shouldBeActive = Mathf.Abs(microgame.transform.position.y) < disableYThreshold;
            if (MenuPracticeMicrogame.SelectedInstance == microgame)
                shouldBeActive = true;
            if (isActive != shouldBeActive)
                microgame.gameObject.SetActive(!isActive);
        }
    }

    void spawnPrefab(float x, float y, int index, bool isBoss)
    {
        var newMicrogame = Instantiate(microgamePrefab, parentObject);
        newMicrogame.GetComponent<MenuButton>().SfxSource = buttonSfxSource;
        newMicrogame.transform.SetSiblingIndex(index);
        newMicrogame.transform.localPosition = new Vector3(x, y, zLevel);
        newMicrogame.name = !isBoss ?
            $"Microgame ({index.ToString()})"
            : $"Boss ({index.ToString()})";
        newMicrogame.NameText = nameText;
        newMicrogame.CreditsTexts = creditsTexts;
        newMicrogame.RootMenu = rootMenu;
        if (newMicrogame.microgame != null)
            newMicrogame.SetSprite(microgameIcons.FirstOrDefault(a => a.name.Contains(newMicrogame.microgame.microgameId)));
        spawnedMicrogames.Add(newMicrogame);
    }
}