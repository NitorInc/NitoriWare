#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace FoodRoast {
  public class ReadOnlyAttribute : PropertyAttribute {

  }

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
  public class PotatoCookTimeProperty : PropertyDrawer{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
      return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      var size = position.size;
      size.x /= 2;

      EditorGUI.PropertyField(new Rect(position.position, size), property.FindPropertyRelative("minTime"), new GUIContent("Min"));
      EditorGUI.PropertyField(new Rect(position.position + new Vector2(size.x, 0), size), property.FindPropertyRelative("maxTime"), new GUIContent("Max"));
    }
  }

}
#endif
