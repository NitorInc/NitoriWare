using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(MicrogameController))]
[CanEditMultipleObjects]
public class MicrogameControllerEditor : Editor
{
    //SerializedProperty lookAtPoint;

    MicrogameController microgameController => target as MicrogameController;

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit Microgame Traits"))
        {
            var microgames = MicrogameCollection.LoadAllMicrogames();
            Selection.activeObject = microgames.FirstOrDefault(a =>
                microgameController.gameObject.scene.name.Contains(a.name));
        }
        DrawDefaultInspector();
    }
}
