using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuGameMode : MonoBehaviour
{
    private const string KeyPrefix = "menu.gamemode.";

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private MenuButton menuButton;
    [SerializeField]
    private MenuDescriptionText descriptionText;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private GameplayMenu gameplayMenu;
    [SerializeField]
    private string modeName;
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
        formattedLanguage = TextHelper.getLoadedLanguage();
    }

    void Update()
	{
        if (TextHelper.getLoadedLanguage() != formattedLanguage)
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
        gameplayMenu.startGameplay(modeName);
    }
}
