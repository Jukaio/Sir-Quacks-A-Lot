using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml;

// Possible player input commands
// Assign saved player inputs from XML
// For single device usage per player, just have one device in the XML (Use a switch-case or something
namespace Game_Input
{
    using Input_Commands;

    public class Player_Input_System : MonoBehaviour
    {
        public TextAsset m_file;

        static private Player_Input_System[] m_instance = new Player_Input_System[0];
        static public Player_Input_System Player(int index)
        {
            return m_instance[index];
        }

        public Command[] m_commands;
        private All_Controls m_controls;

        public Device m_current_device;
        public bool Move_Left
        {
            get
            {
                return m_commands[(int)Commands.MOVE_LEFT].Is_Pressed(ref m_current_device);
            }
        }
        public bool Move_Right
        {
            get
            {
                return m_commands[(int)Commands.MOVE_RIGHT].Is_Pressed(ref m_current_device);
            }
        }
        public bool Move_Up
        {
            get
            {
                return m_commands[(int)Commands.MOVE_UP].Is_Pressed(ref m_current_device);
            }
        }
        public bool Move_Down
        {
            get
            {
                return m_commands[(int)Commands.MOVE_DOWN].Is_Pressed(ref m_current_device);
            }
        }

        private void Awake()
        {
            m_commands = new Command[4]; // Maximal mapped buttons
            for (int j = 0; j < 4; j++)
            {
                m_commands[j] = new Command();
            }
            Deserialize_XML_File();
        }

        void Deserialize_XML_File()
        {
            int index = Create_Player_Input();       
            m_controls = XML_Serializer.Deserialize<All_Controls>(m_file);
            Assign_Controls(m_controls);
        }

        void Assign_Controls(All_Controls p_layout)
        {
            if (p_layout.Keyboard != null)
            {
                m_commands[(int)Commands.MOVE_LEFT].Set_Key(p_layout.Keyboard.MOVE_LEFT);
                m_commands[(int)Commands.MOVE_RIGHT].Set_Key(p_layout.Keyboard.MOVE_RIGHT);
                m_commands[(int)Commands.MOVE_UP].Set_Key(p_layout.Keyboard.MOVE_UP);
                m_commands[(int)Commands.MOVE_DOWN].Set_Key(p_layout.Keyboard.MOVE_DOWN);
            }
            if (p_layout.Playstation != null)
            {
                m_commands[(int)Commands.MOVE_LEFT].Set_Playstation_Button(p_layout.Playstation.MOVE_LEFT);
                m_commands[(int)Commands.MOVE_RIGHT].Set_Playstation_Button(p_layout.Playstation.MOVE_RIGHT);
                m_commands[(int)Commands.MOVE_UP].Set_Playstation_Button(p_layout.Playstation.MOVE_UP);
                m_commands[(int)Commands.MOVE_DOWN].Set_Playstation_Button(p_layout.Playstation.MOVE_DOWN);
            }
            if (p_layout.Xbox != null)
            {
                m_commands[(int)Commands.MOVE_LEFT].Set_XBOX_Button(p_layout.Xbox.MOVE_LEFT);
                m_commands[(int)Commands.MOVE_RIGHT].Set_XBOX_Button(p_layout.Xbox.MOVE_RIGHT);
                m_commands[(int)Commands.MOVE_UP].Set_XBOX_Button(p_layout.Xbox.MOVE_UP);
                m_commands[(int)Commands.MOVE_DOWN].Set_XBOX_Button(p_layout.Xbox.MOVE_DOWN);
            }
        }

        private int Create_Player_Input()
        {
            m_commands = new Command[4];
            for (int j = 0; j < 4; j++)
            {
                m_commands[j] = new Command();
            }

            Player_Input_System[] temp = m_instance;
            m_instance = new Player_Input_System[m_instance.Length + 1];

            int index;
            for (index = 0; index < temp.Length; index++)
                m_instance[index] = temp[index];
            m_instance[index] = this;
            return index;
        }
    }
}