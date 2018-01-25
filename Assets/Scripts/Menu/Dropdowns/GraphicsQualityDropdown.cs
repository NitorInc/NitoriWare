using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicsQualityDropdown : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private string keyPrefix;
    [SerializeField]
    private Text label;
#pragma warning restore 0649

    void Start()
    {
        if (!string.IsNullOrEmpty(TextHelper.getLoadedLanguageID()))
            setOptionText();

        dropdown.value = QualitySettings.GetQualityLevel();
    }

    public void setOptionText()
    {
        string[] defaultNames = QualitySettings.names;
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            dropdown.options[i].text = TextHelper.getLocalizedText(keyPrefix + "." + i.ToString(), defaultNames[i]);
        }
        label.text = dropdown.options[dropdown.value].text;
    }

    public void select(int item)
    {
        if (QualitySettings.GetQualityLevel() != item)
            QualitySettings.SetQualityLevel(item);
    }
}
