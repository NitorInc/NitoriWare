using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizationUpdater))]
public class LocalizationUpdaterEditor : Editor
{
    LocalizationUpdater updater;

    private void OnEnable()
    {
        updater = (LocalizationUpdater)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("To add or edit language info, use Languages Data.");

        GUILayout.Label("Update all languages, takes a while:");
        if (GUILayout.Button("Update Language Content"))
        {
            updater.updateLanguages();
        }
        
        DrawDefaultInspector();
    }
}
