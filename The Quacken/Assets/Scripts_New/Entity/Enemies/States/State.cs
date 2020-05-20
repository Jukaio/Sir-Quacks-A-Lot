using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base
public abstract class State<T> : ScriptableObject
{
    public string m_name;
    [System.NonSerialized] public string m_next;
    private protected T m_context;

    public void Init(T p_context)
    {
        m_context = p_context;
        Init();
    }
    public abstract void Init();
    public abstract void Enter();
    public abstract bool Run();
    public abstract void Animate();
    public abstract void Exit();
}
