using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocalizationUpdaterWindow : EditorWindow
{
    bool expandFonts = true;
    List<TMPFont> selectedFonts;
    Vector2 scrollPos;
    LocalizationUpdater updater;
    
    [MenuItem("Window/NitorInc./Release Prep/Localization Updater")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(LocalizationUpdaterWindow));
    }

    private void OnEnable()
    {
        selectedFonts = new List<TMPFont>();
        titleContent = new GUIContent("Localization Updater");
        updater = ((LocalizationUpdater)EditorGUIUtility.Load("Localization Updater.asset"));
    }

    void OnGUI()
    {
        var headerStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter , fontStyle = FontStyle.Bold};
        var boldStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

        GUILayout.Label("Check console for results.", headerStyle);
        GUILayout.Label("");

        scrollPos = GUILayout.BeginScrollView(scrollPos);


        GUILayout.Label("STEP 1 - Language Content", headerStyle);
        GUILayout.Label("Pulls all language values from Google sheet, takes a while.");
        GUILayout.Label("Needs an internet connection, obviously.");
        GUILayout.Label("TO ADD OR EDIT LANGUAGES, edit Languages Data.", boldStyle);
        if (GUILayout.Button("Update Language Content"))
        {
            updater.updateLanguages();
        }

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("STEP 2 - Character Files", headerStyle);
        GUILayout.Label("Update char files for fonts.");
        GUILayout.Label("Call this after Update Language Content.");
        if (GUILayout.Button("Generate Chars Files."))
        {
            updater.updateCharsFiles();
        }

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("STEP 3 - Font Character Verification", headerStyle);
        GUILayout.Label("Check whether TMP FontAssets are missing characters.");
        GUILayout.Label("Checks all languages each font is compatible with.");
        GUILayout.Label("Compatibility data in metadata page of localization sheet.");
        GUILayout.Label("CHAR FILES MUST BE GENERATED FIRST.", boldStyle);
        if (GUILayout.Button("Check Font Chars"))
        {
            updater.checkFontChars();
        }
        GUILayout.Label("Check console for output.");

        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label("STEP 4 - Font Atlases", headerStyle);
        GUILayout.Label("Helps load each font with all the characters it needs in the game");
        GUILayout.Label("based on compatability data in sheet.");
        GUILayout.Label("Use before builds to prevent dynamic repacking.");
        GUILayout.Label("CHAR FILES MUST BE GENERATED FIRST.", boldStyle);
        GUILayout.Label("TO ADD OR EDIT FONTS, edit TMP Fonts Data.", boldStyle);

        if (GUILayout.Button("Update All Fonts With Missing Chars"))
        {
            updater.rebuildAllIncompleteFontAtlases();
        }
        GUILayout.Label("Check console for important notes post-update.");

        GUILayout.Label("");
        GUILayout.Label("Update Individual fonts.");
        if (GUILayout.Button("Update Selected Font(s)"))
        {
            foreach (var font in selectedFonts)
            {
                updater.rebuildFontAtlas(font);
            }
            selectedFonts.Clear();
        }
        foreach (var font in TMPFontsData.instance.fonts)
        {

            var wasSelected = selectedFonts.Contains(font);
            var isSelected = GUILayout.Toggle(wasSelected, font.assetName);

            if (isSelected && !wasSelected)
                selectedFonts.Add(font);
            else if (wasSelected && !isSelected)
                selectedFonts.Remove(font);
        }

        GUILayout.EndScrollView();
    }
}