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
#endif
