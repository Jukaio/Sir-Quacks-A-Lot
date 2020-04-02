using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    private KeyCode[] m_key_codes = new KeyCode[0];

    public Command()
    {
        m_key_codes = new KeyCode[0];
    }
    public Command(List<KeyCode> keys)
    {
        m_key_codes = new KeyCode[0];
        foreach (KeyCode key in keys)
            Push_Back(key);
    }
    public Command(KeyCode key_code)
    {
        m_key_codes = new KeyCode[0];
        Push_Back(key_code);
    }

    public void Push_Back(KeyCode key)
    {
        KeyCode[] temp = m_key_codes;
        m_key_codes = new KeyCode[m_key_codes.Length + 1];

        int index;
        for (index = 0 ; index < temp.Length; index++)
            m_key_codes[index] = temp[index];
        m_key_codes[index] = key;
    }

    public bool Is_Pressed()
    {
        foreach (KeyCode key in m_key_codes)
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }
}
