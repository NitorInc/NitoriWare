using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RumiaRescueMapGenerater : MonoBehaviour {
    [SerializeField]
    private GameObject rumia;
    [SerializeField]
    private GameObject[] needHelpList;
    [SerializeField]
    private GameObject[] obstacleList;

    [SerializeField]
    private int needHelpCount;
    [SerializeField]
    private int obstacleCount;

    [SerializeField]
    private const string positionDataRelativePath = "/Microgames/RumiaRescue/ScriptableObjects/";

    private void Start() {
        string fileName = "Traits";
        string extension = ".asset";
        print(Application.dataPath);    
        var positionDataDirectories = Directory.GetDirectories(Application.dataPath + positionDataRelativePath);
        print("!");
        foreach(string s in positionDataDirectories)
            print(s);
        /*
        MicrogameTraits traits = AssetDatabase.LoadAssetAtPath<MicrogameTraits>($"Assets{positionDataRelativePath}/{fileName}{extension}");
        if (traits != null)
            return traits;
            */
        
    }
}
