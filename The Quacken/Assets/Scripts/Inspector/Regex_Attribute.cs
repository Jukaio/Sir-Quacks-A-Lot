using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

public class Regex_Attribute : PropertyAttribute
{

    public readonly string m_pattern;
    public readonly string m_help_message;

    public Regex_Attribute(string p_pattern, string p_help_message)
    {
        this.m_pattern = p_pattern;
        this.m_help_message = p_help_message;
    }
}
