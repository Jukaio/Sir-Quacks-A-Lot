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
    }

    public class Controls_Controller : Controls
    {
        public string axis_up_and_down;
        public string axis_left_and_right;
    }

    public class Controls_Playstation : Controls
    {
        public PS_Button MOVE_LEFT;
        public PS_Button MOVE_RIGHT;
        public PS_Button MOVE_UP;
        public PS_Button MOVE_DOWN;
    }

    public class Controls_Xbox : Controls
    {
        public XBOX_Button MOVE_LEFT;
        public XBOX_Button MOVE_RIGHT;
        public XBOX_Button MOVE_UP;
        public XBOX_Button MOVE_DOWN;
    }


    public class All_Controls
    {
        public Controls_Keyboard Keyboard;
        public Controls_Playstation Playstation;
        public Controls_Xbox Xbox;
    }
}