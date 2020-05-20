using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Clip))]
public class Clip_Display : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        // Calculate rects
        Rect name_label_rect = new Rect(position.x, position.y, 115 - 95, position.height);
        Rect name_rect = new Rect(position.x + 20, position.y, 115 - 20, position.height);
        Rect clip_rect = new Rect(position.x + 120, position.y, position.width - 140, position.height);
        Rect loop_rect = new Rect(position.x + position.width - 20, position.y, 20, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.LabelField(name_label_rect, "ID:");
        EditorGUI.PropertyField(name_rect, property.FindPropertyRelative("m_name"), GUIContent.none);
        EditorGUI.PropertyField(clip_rect, property.FindPropertyRelative("m_clip"), GUIContent.none);
        EditorGUI.PropertyField(loop_rect, property.FindPropertyRelative("m_loop"), GUIContent.none);


        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
