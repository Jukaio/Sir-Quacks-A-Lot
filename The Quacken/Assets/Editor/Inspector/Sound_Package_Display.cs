using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Sound_Package), true)]
public class Sound_Package_Display : PropertyDrawer
{
    float m_text_height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        m_text_height = EditorGUI.GetPropertyHeight(property, label, false);
        float other_height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_clips"), true);
        return EditorGUI.GetPropertyHeight(property, label, false) + other_height;
    }

    //Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect name_label_rect = new Rect(position.x, position.y, 115 - 75, m_text_height);
        Rect name_rect = new Rect(position.x + 40, position.y, position.width - 40, m_text_height);
        Rect clip_rect = new Rect(position.x, position.y, position.width, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.LabelField(name_label_rect, "Name:");
        EditorGUI.PropertyField(name_rect, property.FindPropertyRelative("m_package_name"), GUIContent.none);
        EditorGUI.PropertyField(clip_rect, property.FindPropertyRelative("m_clips"), GUIContent.none, true);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
    
}
