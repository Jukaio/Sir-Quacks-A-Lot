using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml.Serialization;

// Possible player input commands
public enum Commands
{
    MOVE_LEFT,
    MOVE_RIGHT,
    MOVE_UP,
    MOVE_DOWN,
}

// Assign saved player inputs from XML
// For single device usage per player, just have one device in the XML (Use a switch-case or something
public class Player_Input : MonoBehaviour
{

    static private Player_Input[] m_instance = new Player_Input[0];
    static public Player_Input Player(int index)
    {
        return m_instance[index];
    }

    public Command[] m_commands;

    public Device m_current_device;
    public bool m_move_left
    {
        get
        {
            return m_commands[(int)Commands.MOVE_LEFT].Is_Pressed(ref m_current_device);
        }
    }
    public bool m_move_right
    {
        get
        {
            return m_commands[(int)Commands.MOVE_RIGHT].Is_Pressed(ref m_current_device);
        }
    }
    public bool m_move_up
    {
        get
        {
            return m_commands[(int)Commands.MOVE_UP].Is_Pressed(ref m_current_device);
        }
    }
    public bool m_move_down
    {
        get
        {
            return m_commands[(int)Commands.MOVE_DOWN].Is_Pressed(ref m_current_device);
        }
    }

    private void Awake()
    {
        m_commands = new Command[4];
        for (int j = 0; j < 4; j++)
        {
            m_commands[j] = new Command();
        }
        Deserialize_XML_File();
    }

    void Deserialize_XML_File()
    {
        int index = Create_Player_Input();
        string path = "Assets/Configs/Player_Inputs_" + index + ".xml";
        All_Controls layout = XML_Serializer.Deserialize<All_Controls>(path);
        Assign_Controls(layout);
    }

    void Assign_Controls(All_Controls p_layout)
    {
        if (p_layout.Keyboard != null)
        {
            m_commands[(int)Commands.MOVE_LEFT].Push_Back(p_layout.Keyboard.MOVE_LEFT);
            m_commands[(int)Commands.MOVE_RIGHT].Push_Back(p_layout.Keyboard.MOVE_RIGHT);
            m_commands[(int)Commands.MOVE_UP].Push_Back(p_layout.Keyboard.MOVE_UP);
            m_commands[(int)Commands.MOVE_DOWN].Push_Back(p_layout.Keyboard.MOVE_DOWN);
        }
        if (p_layout.Playstation != null)
        {
            m_commands[(int)Commands.MOVE_LEFT].Push_Back(p_layout.Playstation.axis_left_and_right, -1, Device.PLAYSTATION);
            m_commands[(int)Commands.MOVE_RIGHT].Push_Back(p_layout.Playstation.axis_left_and_right, 1, Device.PLAYSTATION);
            m_commands[(int)Commands.MOVE_UP].Push_Back(p_layout.Playstation.axis_up_and_down, -1, Device.PLAYSTATION);
            m_commands[(int)Commands.MOVE_DOWN].Push_Back(p_layout.Playstation.axis_up_and_down, 1, Device.PLAYSTATION);
        }
        if (p_layout.Xbox != null)
        {
            m_commands[(int)Commands.MOVE_LEFT].Push_Back(p_layout.Xbox.axis_left_and_right, -1, Device.XBOX);
            m_commands[(int)Commands.MOVE_RIGHT].Push_Back(p_layout.Xbox.axis_left_and_right, 1, Device.XBOX);
            m_commands[(int)Commands.MOVE_UP].Push_Back(p_layout.Xbox.axis_up_and_down, -1, Device.XBOX);
            m_commands[(int)Commands.MOVE_DOWN].Push_Back(p_layout.Xbox.axis_up_and_down, 1, Device.XBOX);
        }
    }


    private int Create_Player_Input()
    {
        m_commands = new Command[4];
        for (int j = 0; j < 4; j++)
        {
            m_commands[j] = new Command();
        }

        Player_Input[] temp = m_instance;
        m_instance = new Player_Input[m_instance.Length + 1];

        int index;
        for (index = 0; index < temp.Length; index++)
            m_instance[index] = temp[index];
        m_instance[index] = this;
        return index;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
