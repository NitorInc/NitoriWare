#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FoodRoast{
  [CustomEditor(typeof(FoodRoast_Controller))]
  public class FoodRoast_ControllerEditor : Editor {

    public override void OnInspectorGUI() {
      serializedObject.Update();

      EditorGUILayout.PropertyField(serializedObject.FindProperty("AmbienceClips"), true);

      var potatoesExist = serializedObject.FindProperty("PotatoesExists");
      var cookedPotatoesRequirement = serializedObject.FindProperty("CookedPotatoesRequirement");
      EditorGUILayout.PropertyField(potatoesExist);
      EditorGUILayout.PropertyField(cookedPotatoesRequirement);
      if (potatoesExist.intValue < cookedPotatoesRequirement.intValue){
        EditorGUILayout.HelpBox("Impossible to complete cooked potato requirement", MessageType.Error);
      }

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Cook Times");
      var cookTimes = serializedObject.FindProperty("PotatoCookTimes");
      for (var i = 0; i < cookTimes.arraySize; i++){
        EditorGUILayout.PropertyField(cookTimes.GetArrayElementAtIndex(i));
      }

      if (GUILayout.Button("Update Potatoes")) {
        ((FoodRoast_Controller)target).UpdatePotatoes();
      }

      EditorGUILayout.PropertyField(serializedObject.FindProperty("UncookedPotatoesCollected"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("CookedPotatoesCollected"));

      serializedObject.ApplyModifiedProperties();
    }

  }
}
#endif