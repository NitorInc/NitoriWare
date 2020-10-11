using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/TouhouSort/Category")]
public class TouhouSortCategory : ScriptableObject
{
    [SerializeField]
    private string idName;
    public string IdName => idName;

    [SerializeField]
    private Sprite[] leftPool;
    public Sprite[] LeftPool => leftPool;

    [SerializeField]
    private Sprite[] rightPool;
    public Sprite[] RightPool => rightPool;

    [SerializeField]
    private Sprite[] leftPoolNonCanon;
    public Sprite[] LeftPoolNonCanon => leftPoolNonCanon;
    
    [SerializeField]
    private Sprite[] rightPoolNonCanon;
    public Sprite[] RightPoolNonCanon => rightPoolNonCanon;
}
