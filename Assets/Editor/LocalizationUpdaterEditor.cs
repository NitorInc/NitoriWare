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
        GUILayout.Label("Update all languages, takes a while:");
        if (GUILayout.Button("Update Language Content"))
        {
            updater.updateLanguages();
        }

        GUILayout.Label("");
        GUILayout.Label("Update char files for fonts");
        GUILayout.Label("(call this after Update Language Content):");
        if (GUILayout.Button("Update Chars Files"))
        {
            updater.updateCharsFiles();
        }

        GUILayout.Label("");
        GUILayout.Label("Check and log whether TMP FontAssets are");
        GUILayout.Label("missing characters and have to be rebuilt");
        GUILayout.Label("(call this after Update Chars Files):");
        if (GUILayout.Button("Check Font Chars"))
        {
            updater.checkFontChars();
        }

        GUILayout.Label("");
        GUILayout.Label("TO ADD OR EDIT LANGUAGES OR FONTS THEMSELVES, edit Languages Data.");
        GUILayout.Label("Same with adding fonts.");
        GUILayout.Label("----------------------");
        DrawDefaultInspector();
    }
}
