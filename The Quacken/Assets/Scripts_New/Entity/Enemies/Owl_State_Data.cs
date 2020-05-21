using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "FSM/Machine/Owl")]
public class Owl_State_Data : ScriptableObject
{
    public List<Owl_State> m_states;
}