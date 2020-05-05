using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Service<T>
{
    private static T m_instance = default(T);

    static public bool Set(T p_instance)
    {
        if (m_instance == null)
        {
            m_instance = p_instance;
            return true;
        }
        return false;
    }

    static public T Get()
    {
        return m_instance;
    }


}
