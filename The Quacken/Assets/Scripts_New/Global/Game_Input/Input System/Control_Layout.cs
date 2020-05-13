using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Input
{
    using Devices;
    // XML layout
    public class Controls
    {
        public Device device;
    }

    public class Controls_Keyboard : Controls
    {
        public KeyCode MOVE_LEFT;
        public KeyCode MOVE_RIGHT;
        public KeyCode MOVE_UP;
        public KeyCode MOVE_DOWN;
        public KeyCode ACTION;
    }

    public class Controls_Playstation : Controls
    {
        public PS_Button MOVE_LEFT;
        public PS_Button MOVE_RIGHT;
        public PS_Button MOVE_UP;
        public PS_Button MOVE_DOWN;
        public PS_Button ACTION;
    }

    public class Controls_Xbox : Controls
    {
        public XBOX_Button MOVE_LEFT;
        public XBOX_Button MOVE_RIGHT;
        public XBOX_Button MOVE_UP;
        public XBOX_Button MOVE_DOWN;
        public XBOX_Button ACTION;
    }


    public class All_Controls
    {
        public Controls_Keyboard Keyboard;
        public Controls_Playstation Playstation;
        public Controls_Xbox Xbox;

        public static All_Controls Get_Default()
        {
            All_Controls temp = new All_Controls();
            temp.Keyboard.device = Device.KEYBOARD;
            temp.Keyboard.MOVE_LEFT = KeyCode.A;
            temp.Keyboard.MOVE_RIGHT = KeyCode.D;
            temp.Keyboard.MOVE_UP = KeyCode.W;
            temp.Keyboard.MOVE_DOWN = KeyCode.S;
            temp.Keyboard.ACTION = KeyCode.Space;

            return temp;
        }
    }
}