using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class TMPFont
{
    const string FontAssetsPath = "FontAssets/";

    [UnityEngine.Serialization.FormerlySerializedAs("idName")]
    public string assetName;
    public bool isGlobal;
    public BakeData bakeData;
    [System.Serializable]
    public class BakeData
    {
        public GlyphOverride[] glyphOverrides;
        [Multiline]
        public string notes;
    }

    [System.Serializable]
    public class GlyphOverride
    {
        public int id;
        public float W;
        public float H;
        public float OX;
        public float OY;
        public float ADV;
        public float SF;
    }

    public TMP_FontAsset LoadFontAsset() => Resources.Load<TMP_FontAsset>(FontAssetsPath + assetName);

    public ResourceRequest LoadFontAssetAsync() => Resources.LoadAsync<TMP_FontAsset>(FontAssetsPath + assetName);

}
