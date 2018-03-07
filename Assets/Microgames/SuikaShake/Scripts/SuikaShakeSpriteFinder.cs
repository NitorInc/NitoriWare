using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class SuikaShakeSpriteFinder : MonoBehaviour
{
    [Header("Put the texture of your split sprite below and check \"Reload Sprite\" to unpack it")]
    [SerializeField]
    private Texture spriteTexture;
    [SerializeField]
    private bool reloadSprite = true;

    [Header("Your sprites will appear here and can be accessed publicly")]
    public Sprite[] sprites;

    void Start ()
    {
#if !UNITY_EDITOR
        enabled = false;
        return;
#endif
    }
    
	void Update ()
    {
#if UNITY_EDITOR
        if (reloadSprite)
        {
            string spriteSheet = AssetDatabase.GetAssetPath(spriteTexture);
            sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();

            reloadSprite = false;
        }
#endif
    }
}
