using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Input
{
    using Devices;

    public enum Device
    {
        NONE,
        KEYBOARD,
        PLAYSTATION,
        XBOX
    }

    // Command script without deriviation features
    public class Command
    {
        private KeyCode m_key_code = new KeyCode();
        private PS_Button m_ps_buttons_code = new PS_Button();
        private XBOX_Button m_xbox_button_code = new XBOX_Button();

        public Command()
        {
            m_key_code = KeyCode.None;
            m_ps_buttons_code = PS_Button.NONE;
            m_xbox_button_code = XBOX_Button.NONE;
        }
        public void Set_Key(KeyCode p_key_code)
        {
            m_key_code = p_key_code;
        }
        public void Set_Playstation_Button(PS_Button p_ps_buttons_code)
        {
            m_ps_buttons_code = p_ps_buttons_code;
        }
        public void Set_XBOX_Button(XBOX_Button p_xbox_button_code)
        {
            m_xbox_button_code = p_xbox_button_code;
        }

        public bool Is_Pressed(ref Device p_current_device)
        {
            return Playstation.Get_Button(m_ps_buttons_code) ||
                    Xbox.Get_Button(m_xbox_button_code) ||
                    Keyboard.Get_Button(m_key_code);
        }
    }
}
