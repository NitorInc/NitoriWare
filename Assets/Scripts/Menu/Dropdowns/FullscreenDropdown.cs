using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FullscreenDropdown : MonoBehaviour
{
    private const char ResolutionDelimiter = 'x';

#pragma warning disable 0649
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private string yesKey, noKey;
    [SerializeField]
    private Text label;
#pragma warning restore 0649

    void Start()
    {
        setOptionText();
        dropdown.value = Screen.fullScreen ? 0 : 1;
    }

    public void setOptionText()
    {
        dropdown.options[0].text = TextHelper.getLocalizedText(yesKey, "On");
        dropdown.options[1].text = TextHelper.getLocalizedText(noKey, "Off");
        label.text = dropdown.options[dropdown.value].text;
    }

    void Update()
    {
        dropdown.value = Screen.fullScreen ? 0 : 1;
    }

    public void select(int item)
    {
        bool selectedFullscreen = item == 0;
        if (Screen.fullScreen != selectedFullscreen)
            Screen.fullScreen = selectedFullscreen;
    }
}
