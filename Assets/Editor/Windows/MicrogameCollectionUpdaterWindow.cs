using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MicrogameCollectionUpdaterWindow : EditorWindow
{
    Vector2 scrollPos;

    private List<MicrogameList> microgames;
    private class MicrogameList
    {
        public string milestoneName;
        public bool show = false;
        public List<string> labelList;

        public MicrogameList()
        {
            labelList = new List<string>();
        }
    }
    
    [MenuItem("Window/NitorInc./Build Prep/Microgame Build Prep")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MicrogameCollectionUpdaterWindow));
    }

    private void OnEnable()
    {
        titleContent = new GUIContent("Microgame Build Prep");
        setMicrogames();
    }

    void setMicrogames()
    {
        microgames = new List<MicrogameList>();
        var collectionMicrogames = MicrogameCollection.instance.microgames;
        var milestoneNames = Enum.GetNames(typeof(MicrogameTraits.Milestone));
        for (int i = milestoneNames.Length - 1; i >= 0; i--)
        {
            var milestoneMicrogames = collectionMicrogames.Where(a => (int)a.traits.milestone == i);
            var newList = new MicrogameList();
            newList.milestoneName = milestoneNames[i];
            foreach (var milestoneMicrogame in milestoneMicrogames)
            {
                string label = milestoneMicrogame.microgameId;
                if (milestoneMicrogame.traits.isBossMicrogame())
                    label += " (BOSS)";
                newList.labelList.Add(label);
            }
            microgames.Add(newList);
        }
    }

    void OnGUI()
    {
        var headerStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter , fontStyle = FontStyle.Bold};
        var boldStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };
        
        GUILayout.Label("Check console for results.", headerStyle);
        GUILayout.Label("");
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        
        GUILayout.Label("STEP 1 - Microgame Collection", headerStyle);
        GUILayout.Label("Updates internal data about where to find newly-added microgames.");
        if (GUILayout.Button("Update Microgame Collection"))
        {
            MicrogameCollection.instance.updateMicrogames();
            setMicrogames();
            EditorUtility.SetDirty(MicrogameCollection.instance);
        }

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("STEP 2 - Build Path", headerStyle);
        GUILayout.Label("Auto-adjusts the game's build settings to include any new microgames.");
        GUILayout.Label("Microgame milestone must be marked as stage-ready or higher to be included.", boldStyle);
        if (GUILayout.Button("Update Build Path"))
        {
            MicrogameCollection.instance.updateBuildPath();
        }

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("STEP 3 - Streaming Music Assets", headerStyle);
        GUILayout.Label("To preserve memory when preloading microgames, microgame music is streamed from disk.");
        GUILayout.Label("This just helps us save some time by doing that for us.");
        if (GUILayout.Button("Set Microgame Music to Streaming"))
        {
            MicrogameCollection.instance.SetMicrogameMusicToStreaming();
        }

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("Indexed Microgames:");
        var names = Enum.GetNames(typeof(MicrogameTraits.Milestone));
        foreach (var microgameList in microgames)
        {
            microgameList.show = EditorGUILayout.Foldout(microgameList.show, microgameList.milestoneName + $" ({microgameList.labelList.Count().ToString()})");
            if (microgameList.show)
            {
                foreach (var microgameLabel in microgameList.labelList)
                {
                    EditorGUILayout.LabelField("- " + microgameLabel);
                }
            }
        }

        GUILayout.EndScrollView();
    }
}