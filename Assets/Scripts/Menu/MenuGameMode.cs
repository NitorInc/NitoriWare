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
    private string modeName;
    [SerializeField]
    private bool triggerDescription;

    [SerializeField]
    [Multiline]
    private string defaultDescription;
#pragma warning restore 0649

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
        highScoreText.text = string.Format(TextHelper.getLocalizedText("stage.gameover.highscore", highScoreText.text),
            PrefsHelper.getHighScore(modeName).ToString("D3"));
        //highScoreText.text = highScoreText.text
    }

    void Update()
	{
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
}
