using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "FSM/Machine/Pig")]
public class Pig_State_Data : ScriptableObject
{
    public List<Pig_State> m_states;
}