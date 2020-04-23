using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Board_Button
{
    public KeyCode m_button;
    public string m_pack_name;
    public string m_sound_name;
}

public class Sound_Board : MonoBehaviour
{
    public Board_Button[] m_board_buttons;

    void Start()
    {
        
    }

    void Update()
    {
        foreach (Board_Button button in m_board_buttons)
        {
            if (Input.GetKeyDown(button.m_button))
                Service<Sound_Manager>.Get().Play(button.m_pack_name, button.m_sound_name);
        }
    }
}
