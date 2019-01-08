
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FoodRoast{

  using UnityEngine;
  using UnityEditor;

  namespace FoodRoast {
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer {
      public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
      }

      public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
      }
    }

    [CustomPropertyDrawer(typeof(PotatoCookTime))]
    public class PotatoCookTimeProperty : PropertyDrawer {
      public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label);
      }

      public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.LabelField(new Rect(position.position, new Vector2(EditorGUIUtility.labelWidth, position.size.y)), label);

        var size = position.size;
        size.x -= EditorGUIUtility.labelWidth;
        size.x /= 2;

        var minTime = property.FindPropertyRelative("minTime");
        var maxTime = property.FindPropertyRelative("maxTime");
        minTime.floatValue = EditorGUI.FloatField(new Rect(position.position + new Vector2(EditorGUIUtility.labelWidth, 0), size), minTime.floatValue);
        maxTime.floatValue = EditorGUI.FloatField(new Rect(position.position + new Vector2(EditorGUIUtility.labelWidth + size.x, 0), size), maxTime.floatValue);
      }
    }

  }

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
