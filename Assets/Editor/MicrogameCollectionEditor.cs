using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MicrogameCollection))]
public class MicrogameCollectionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MicrogameCollection collection = (MicrogameCollection)target;
		if (GUILayout.Button("Update Microgames"))
		{
			collection.updateMicrogames();
            EditorUtility.SetDirty(collection);
        }
        if (GUILayout.Button("Update Build Path"))
        {
            collection.updateBuildPath();
            EditorUtility.SetDirty(collection);
        }
        DrawDefaultInspector();
	}

}
