using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameMode : MonoBehaviour
{
    private const string KeyPrefix = "menu.gamemode.";

#pragma warning disable 0649
    [SerializeField]
    private MenuButton menuButton;
    [SerializeField]
    private MenuDescriptionText descriptionText;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private GameObject blocker;
    [SerializeField]
    private GameObject unlockedText;
    [SerializeField]
    private GameplayMenu gameplayMenu;
    [SerializeField]
    private string modeName;
    [SerializeField]
    private string overrideSceneName;
    [SerializeField]
    private string prerequisiteStage;
    [SerializeField]
    private int prerequisiteScore;
    [SerializeField]
    private bool triggerDescription;

    [SerializeField]
    [Multiline]
    private string defaultDescription;
#pragma warning restore 0649

    private string initialString, formattedLanguage;

    private List<MenuGameMode> neighbors;

	void Awake()
	{
        neighbors = new List<MenuGameMode>();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            MenuGameMode neighbor = transform.parent.GetChild(i).GetComponent<MenuGameMode>();
            if (neighbor != null)
                neighbors.Add(neighbor);
        }

        if (blocker != null)
        {
            var isUnlockedInPrefs = PrefsHelper.isStageUnlocked(modeName) || GameController.instance.ShowcaseMode || PrefsHelper.getVisitedStage(modeName);
            if (isUnlockedInPrefs || PrefsHelper.getHighScore(prerequisiteStage) >= prerequisiteScore)
            {
                if (!isUnlockedInPrefs)
                    PrefsHelper.setStageUnlocked(modeName, true);
                if (!PrefsHelper.getVisitedStage(modeName))
                {
                    unlockedText.SetActive(true);
                }
            }
            else
            {
                blocker.SetActive(true);
                menuButton.gameObject.SetActive(false);
                highScoreText.gameObject.SetActive(false);
                triggerDescription = false;
            }
        }
	}

    private void Start()
    {
        initialString = highScoreText.text;
        formatText();
    }

    void formatText()
    {
        highScoreText.text = string.Format(TextHelper.getLocalizedText("menu.gamemode.highscore", highScoreText.text),
            PrefsHelper.getHighScore(modeName).ToString("D3"));
        formattedLanguage = TextHelper.getLoadedLanguageID();
    }

    void Update()
	{
        if (TextHelper.getLoadedLanguageID() != formattedLanguage)
        {
            highScoreText.text = initialString;
            formatText();
        }

        if (triggerDescription)
        {
            bool highlighted = menuButton.isMouseOver();

            if (highlighted && !descriptionText.isActivated())
                descriptionText.activate(TextHelper.getLocalizedText(KeyPrefix + modeName + ".description", defaultDescription));
            else if (descriptionText.isActivated() && (GameMenu.shifting || !areAnyNeighborsHighlighted(true)))
                descriptionText.deActivate();
        }
    }

    public MenuButton getMenuButton()
    {
        return menuButton;
    }

    bool areAnyNeighborsHighlighted(bool includeSelf)
    {
        foreach (MenuGameMode neighbor in neighbors)
        {
            if (neighbor.getMenuButton().isMouseOver() && (includeSelf || neighbor != this))
                return true;
        }
        return false;
    }

    public void startGameplay()
    {
        gameplayMenu.startGameplay(string.IsNullOrEmpty(overrideSceneName) ? modeName : overrideSceneName);
    }
}
