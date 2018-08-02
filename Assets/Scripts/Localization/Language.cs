using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Language
{
    [SerializeField]
    private string languageID;
    public string languageName;
    public bool isAsian;
    public bool incomplete;
    public bool disableSelect;
    public string overrideFileName;
    public Font overrideFont;
    public bool forceUnbold;
    public TMP_FontAsset tmproFallback;

    public string getFileName()
    {
        return string.IsNullOrEmpty(overrideFileName) ? languageID : overrideFileName;
    }

    public string getLanguageID()
    {
        return languageID;
    }
}
