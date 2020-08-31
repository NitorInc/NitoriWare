using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizationUpdater))]
public class LocalizationUpdaterEditor : Editor
{
    LocalizationUpdater updater;
    bool expandFonts;
    List<TMPFont> selectedFonts;

    private void OnEnable()
    {
        updater = (LocalizationUpdater)target;
        selectedFonts = new List<TMPFont>();
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
        if (GUILayout.Button("Generate Chars Files"))
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
        GUILayout.Label("Rebuild font atlases");
        GUILayout.Label("based on data in TMP Fonts Data");
        GUILayout.Label("NEEDS CHARS FILES TO EXIST");
        GUILayout.Label("Check console for important notes post-update");
        GUILayout.Label("(takes quite a while)");
        expandFonts = EditorGUILayout.Foldout(expandFonts, "Update TMP Font Asset Atlases:");
        if (expandFonts)
        {
            if (GUILayout.Button("All Incomplete"))
            {
                updater.rebuildAllIncompleteFontAtlases();
            }

            GUILayout.Label("Update Individually:");
            if (GUILayout.Button("Selected Font(s)"))
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
                var isSelected = GUILayout.Toggle(wasSelected, font.idName);

                if (isSelected && !wasSelected)
                    selectedFonts.Add(font);
                else if (wasSelected && !isSelected)
                    selectedFonts.Remove(font);
            }
        }

        GUILayout.Label("");
        GUILayout.Label("TO ADD OR EDIT LANGUAGES, edit Languages Data.");
        GUILayout.Label("TO ADD OR EDIT FONTS, edit TMP Fonts Data.");
        GUILayout.Label("----------------------");
        DrawDefaultInspector();
    }
}
