using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DTLocalization.Internal;

[System.Serializable]
public class TMPFont
{
    public string idName;
    public TMP_FontAsset fontAsset;
    public BakeData bakeData;
    [System.Serializable]
    public class BakeData
    {
        public Font baseFont;
        public int fontSize = 86;
        public int padding = 9;
        public string characterTextFile;
        public int atlasWidth = 1024;
        public int atlasHeight = 1024;
        [Multiline]
        public string notes;
    }

}
