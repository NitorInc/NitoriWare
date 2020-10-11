using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Control/TMPro Font Data")]
public class TMPFontsData : ScriptableObjectSingleton<TMPFontsData>
{
    [Header("Update this list manually.")]
    

    [SerializeField]
    private TMPFont[] _fonts;
    public TMPFont[] fonts => _fonts;
}
