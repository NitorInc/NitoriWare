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
		}
		DrawDefaultInspector();
	}

}
