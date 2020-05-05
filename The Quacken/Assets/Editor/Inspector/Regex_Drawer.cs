//using UnityEditor;
//using UnityEngine;
//using System.Text.RegularExpressions;

//[CustomPropertyDrawer(typeof(Regex_Attribute))]
//public class Regex_Drawer : PropertyDrawer
//{
//    const int m_help_height = 30;
//    const int m_text_height = 16;

//    Regex_Attribute m_regex_attribute { get { return ((Regex_Attribute)attribute); } }

//    public override float GetPropertyHeight(SerializedProperty p_prop, GUIContent p_label)
//    {
//        if (Is_Valid(p_prop))
//            return base.GetPropertyHeight(p_prop, p_label);
//        else
//            return base.GetPropertyHeight(p_prop, p_label) + m_help_height;
//    }

//    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
//    {
//        // Adjust height of the text field
//        Rect textFieldPosition = position;
//        textFieldPosition.height = m_text_height;
//        DrawTextField(textFieldPosition, prop, label);

//        // Adjust the help box position to appear indented underneath the text field.
//        Rect helpPosition = EditorGUI.IndentedRect(position);
//        helpPosition.y += m_text_height;
//        helpPosition.height = m_help_height;
//        DrawHelpBox(helpPosition, prop);
//    }

//    void DrawTextField(Rect position, SerializedProperty prop, GUIContent label)
//    {
//        // Draw the text field control GUI.
//        EditorGUI.BeginChangeCheck();
//        string value = EditorGUI.TextField(position, label, prop.stringValue);
//        if (EditorGUI.EndChangeCheck())
//            prop.stringValue = value;
//    }

//    void DrawHelpBox(Rect position, SerializedProperty prop)
//    {
//        // No need for a help box if the pattern is valid.
//        if (Is_Valid(prop))
//            return;

//        EditorGUI.HelpBox(position, m_regex_attribute.m_help_message, MessageType.Error);
//    }

//    bool Is_Valid(SerializedProperty prop)
//    {
//        return Regex.IsMatch(prop.stringValue, m_regex_attribute.m_pattern);
//    }
//}
