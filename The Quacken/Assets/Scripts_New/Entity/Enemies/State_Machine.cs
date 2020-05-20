using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "FSM/Machine/Pig")]
public class Pig_State_Data : ScriptableObject
{
    public List<Pig_State> m_states;
}

public class State_Machine<T> 
{
    [SerializeField] public Dictionary<string, State<T>> m_state_collection = new Dictionary<string, State<T>>();
    public State<T> m_current;
    public State<T> m_previous;

    public void Add(string p_name, State<T> p_state)
    {
        m_state_collection.Add(p_name, p_state);
    }

    public void Run()
    {
        // If no state is set, just return 
        if (m_current == null)
            return;

        // Switches State
        bool switch_state = m_current.Run();
        m_current.Animate();

        if(!switch_state)
        {
            Switch(m_current.m_next);
        }
    }

    private bool Switch(string p_name)
    {
        // Tries to find a value with passed key
        if (m_state_collection.TryGetValue(p_name, out State<T> state))
        {
            if (m_current != null)
                m_current.Exit();
            m_previous = m_current;
            m_current = state;
            m_current.Enter();
            return true;
        }
        Debug.LogError("State does not exist - State_Machine.cs");
        return false;
    }

    public void Set(string p_name)
    {
        if (m_state_collection.TryGetValue(p_name, out State<T> state))
        {
            if (m_current != null)
                m_current.Exit();
            m_current = state;
            m_current.Enter();
            return;
        }
        Debug.LogError("State does not exist - State_Machine.cs");

    }
}
