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

      EditorGUILayout.PropertyField(serializedObject.FindProperty("ambienceClips"), true);

      var potatoesExist = serializedObject.FindProperty("potatoesExists");
      var cookedPotatoesRequirement = serializedObject.FindProperty("cookedPotatoesRequirement");
      EditorGUILayout.PropertyField(potatoesExist);
      EditorGUILayout.PropertyField(cookedPotatoesRequirement);
      if (potatoesExist.intValue < cookedPotatoesRequirement.intValue){
        EditorGUILayout.HelpBox("Impossible to complete cooked potato requirement", MessageType.Error);
      }

      EditorGUILayout.Space();
      EditorGUILayout.PropertyField(serializedObject.FindProperty("potatoBurnTime"));
      EditorGUILayout.LabelField("Potato Cook Times  (Min-Max)");
      var cookTimes = serializedObject.FindProperty("potatoCookTimes");
      for (var i = 0; i < cookTimes.arraySize; i++){
        var element = cookTimes.GetArrayElementAtIndex(i);
        EditorGUILayout.PropertyField(element, new GUIContent(string.Format("Potato {0}", i + 1)));
        if (element.FindPropertyRelative("minTime").floatValue > element.FindPropertyRelative("maxTime").floatValue){
          EditorGUILayout.HelpBox("minValue is higher than maxValue", MessageType.Error);
        }
      }

      if (GUILayout.Button("Update Potatoes")) {
        ((FoodRoast_Controller)target).UpdatePotatoes();
      }

      EditorGUILayout.PropertyField(serializedObject.FindProperty("uncookedPotatoesCollected"));
      EditorGUILayout.PropertyField(serializedObject.FindProperty("cookedPotatoesCollected"));

      serializedObject.ApplyModifiedProperties();
    }

  }
}
#endif