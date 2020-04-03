﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    class Axis_Command
    {
        public Axis_Command() { }
        public Axis_Command(string p_axis_name, int p_direction)
        {
            m_axis_name = p_axis_name;
            m_direction = p_direction;
        }
        public string m_axis_name;
        public int m_direction;
    }

    private KeyCode[] m_key_codes = new KeyCode[0];
    private Axis_Command[] m_axes = new Axis_Command[0];

    public Command()
    {
        m_key_codes = new KeyCode[0];
    }

    public void Push_Back(KeyCode p_key)
    {
        KeyCode[] temp = m_key_codes;
        m_key_codes = new KeyCode[m_key_codes.Length + 1];

        int index;
        for (index = 0 ; index < temp.Length; index++)
            m_key_codes[index] = temp[index];
        m_key_codes[index] = p_key;
    }
    public void Push_Back(string p_axis, int p_direction)
    {
        Axis_Command[] temp = m_axes;
        m_axes = new Axis_Command[m_axes.Length + 1];

        int index;
        for (index = 0; index < temp.Length; index++)
            m_axes[index] = temp[index];
        m_axes[index] = new Axis_Command(p_axis, p_direction);
    }


    public bool Is_Pressed()
    {
        foreach (KeyCode key in m_key_codes)
        {
            if (Input.GetKey(key))
                return true;
        }
        foreach (Axis_Command axis in m_axes)
        {
            if (Input.GetAxisRaw(axis.m_axis_name) == axis.m_direction)
                return true;
        }
        return false;
    }
}
