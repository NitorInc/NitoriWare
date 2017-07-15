using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicsQualityDropdown : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private string keyPrefix;
#pragma warning restore 0649

    void Start()
    {
        if (!string.IsNullOrEmpty(TextHelper.getLoadedLanguage()))
            setOptionText();

        dropdown.value = QualitySettings.GetQualityLevel();
    }

    void setOptionText()
    {
        string[] defaultNames = QualitySettings.names;
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            dropdown.options[i].text = TextHelper.getLocalizedText(keyPrefix + "." + i.ToString(), defaultNames[i]);
        }
    }

    public void select(int item)
    {
        if (QualitySettings.GetQualityLevel() != item)
            QualitySettings.SetQualityLevel(item);
    }
}
