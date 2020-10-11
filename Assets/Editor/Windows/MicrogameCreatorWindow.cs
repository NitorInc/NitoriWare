using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MicrogameCreatorWindow : EditorWindow
{
    Vector2 scrollPos;
    string microgameId = "";
    int selectedType = 0;
    MicrogameCreator creator;

    [MenuItem("Window/NitorInc./Microgame Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MicrogameCreatorWindow));
    }

    private void OnEnable()
    {
        titleContent = new GUIContent("Microgame Generator");
        creator = ((MicrogameCreator)EditorGUIUtility.Load("Microgame Creator.asset"));
    }


    void OnGUI()
    {
        var headerStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        var boldStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

        GUILayout.Label("Check console for results.", headerStyle);
        GUILayout.Label("");
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("Microgame ID will be permanent,", boldStyle);
        GUILayout.Label("please type it in right.", boldStyle);
        microgameId = EditorGUILayout.TextField("Microgame ID:", microgameId);

        GUILayout.Label("");
        GUILayout.Label("Microgame Traits type:");
        selectedType = EditorGUILayout.Popup(selectedType, GetEnumNames());
        GUILayout.Label("Mostly determines scene count.");
        GUILayout.Label("Choose based on what you think will be easiest.");
        GUILayout.Label("This can be changed later.");

        GUILayout.Label("");
        if (GUILayout.Button("Oh! Create!"))
        {
            if (creator.CreateMicrogame(microgameId, (MicrogameCreator.MicrogameType)selectedType))
                Close();
        }

        GUILayout.EndScrollView();
    }

    string[] GetEnumNames() => (Enum.GetValues(typeof(MicrogameCreator.MicrogameType)) as MicrogameCreator.MicrogameType[])
        .Select(a => SpaceOutTypeName(a))
        .ToArray();

    string SpaceOutTypeName(MicrogameCreator.MicrogameType type) =>
        string.Join("",
            type.ToString()
                .SelectMany(a => ((a >= 'A' && a <= 'Z') ? " " : "") + a.ToString()))
        .Trim();
}