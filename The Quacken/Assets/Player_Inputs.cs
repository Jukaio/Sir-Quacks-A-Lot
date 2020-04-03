using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml.Serialization;

public enum Commands
{
    MOVE_LEFT,
    MOVE_RIGHT,
    MOVE_UP,
    MOVE_DOWN,
}

public class Player_Inputs : MonoBehaviour
{
    static private Player_Inputs[] m_instance = new Player_Inputs[0];
    static public Player_Inputs Player(int index)
    {
        return m_instance[index];
    }

    Keyboard m_keyboard;
    Gamepad m_gamepad;

    Dictionary<KeyCode, bool> m_buttons;
    public Command[] m_commands;

    public bool m_move_left
    {
        get
        {
            return m_commands[0].Is_Pressed();
        }
    }

    private void Awake()
    {
        m_commands = new Command[4];
        for (int j = 0; j < 4; j++)
        {
            m_commands[j] = new Command();
        }
        int index = Create_Player_Input();
        string path = "Assets/Configs/Player_Inputs_" + index + ".xml";

        All_Controls layout = XML_Serializer.Deserialize<All_Controls>(path);
        Assign_Controls(layout);
    }

    void Assign_Controls(All_Controls p_layout)
    {
        if (p_layout.Keyboard != null)
        {
            m_commands[0].Push_Back(p_layout.Keyboard.MOVE_LEFT);
            m_commands[1].Push_Back(p_layout.Keyboard.MOVE_RIGHT);
            m_commands[2].Push_Back(p_layout.Keyboard.MOVE_UP);
            m_commands[3].Push_Back(p_layout.Keyboard.MOVE_DOWN);
        }
        if (p_layout.Playstation != null)
        {
            m_commands[0].Push_Back(p_layout.Playstation.axis_left_and_right, -1);
            m_commands[1].Push_Back(p_layout.Playstation.axis_left_and_right, 1);
            m_commands[2].Push_Back(p_layout.Playstation.axis_up_and_down, -1);
            m_commands[3].Push_Back(p_layout.Playstation.axis_up_and_down, 1);
        }
        if (p_layout.Xbox != null)
        {
            m_commands[0].Push_Back(p_layout.Xbox.axis_left_and_right, -1);
            m_commands[1].Push_Back(p_layout.Xbox.axis_left_and_right, 1);
            m_commands[2].Push_Back(p_layout.Xbox.axis_up_and_down, -1);
            m_commands[3].Push_Back(p_layout.Xbox.axis_up_and_down, 1);
        }
    }


    private int Create_Player_Input()
    {
        Player_Inputs[] temp = m_instance;
        m_instance = new Player_Inputs[m_instance.Length + 1];

        int index;
        for (index = 0; index < temp.Length; index++)
            m_instance[index] = temp[index];
        m_instance[index] = this;
        return index;
    }


    private void Start()
    {
        m_buttons = new Dictionary<KeyCode, bool>();
        m_keyboard = (Keyboard)GetComponent(typeof(Keyboard));
        m_gamepad = (Gamepad)GetComponent(typeof(Gamepad));

        //m_commands[0] = new Command();
        //m_commands[0].Push_Back(m_keyboard.m_move_left);
        //m_commands[0].Push_Back(m_gamepad.m_move_left);

        //m_buttons.Add(m_keyboard.m_move_down, false);
        //m_buttons.Add(m_keyboard.m_move_up, false);
        //m_buttons.Add(m_keyboard.m_move_left, false);
        //m_buttons.Add(m_keyboard.m_move_right, false);
        //m_buttons.Add(m_gamepad.m_move_down, false);
        //m_buttons.Add(m_gamepad.m_move_up, false);
        //m_buttons.Add(m_gamepad.m_move_left, false);
        //m_buttons.Add(m_gamepad.m_move_right, false);
    }

    private void Update()
    {
        
    }
}
