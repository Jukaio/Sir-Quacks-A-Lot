using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

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
        string path = "Assets/Configs/Inputs_Player_" + index + ".txt";
        StreamReader reader = new StreamReader(path);

        string[] file = reader.ReadToEnd().Split('\r');
        string[] parts = new string[file.Length - 1];
        for(int i = 0; i < file.Length - 1; i++)
        {
            parts[i] = file[i + 1];
        }


        switch (file[0])
        {
            case "Keyboard:":
                Check_Lines(parts);
                break;

            case "Playstation:":

                break;

            case "XBOX:":

                break;
        }
    }

    void Check_Lines(string[] lines)
    {
        Commands command;
        KeyCode key_code;
        string temp_command;
        string temp_key_code;

        char c;
        int line_index;
        foreach (string line in lines)
        {
            line_index = 1;
            temp_command = "";
            temp_key_code = "";
            c = line[line_index];

            while (c != ' ')
            {
                temp_command += c;
                line_index++;
                c = line[line_index];
            }
            line_index += 2;
            while (c != ';')
            {
                temp_key_code += c;
                line_index++;
                c = line[line_index];
            }


            Enum.TryParse(temp_command, true, out command);
            Enum.TryParse(temp_key_code, true, out key_code);
            switch (command)
            {
                case Commands.MOVE_LEFT:
                    m_commands[0].Push_Back(key_code);
                    break;
                case Commands.MOVE_RIGHT:
                    m_commands[1].Push_Back(key_code);
                    break;
                case Commands.MOVE_UP:
                    m_commands[2].Push_Back(key_code);
                    break;
                case Commands.MOVE_DOWN:
                    m_commands[3].Push_Back(key_code);
                    break;
            }
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
