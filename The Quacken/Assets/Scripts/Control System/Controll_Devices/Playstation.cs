using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

//Control layout for playstation 
public enum PS_Button
{
    NONE = -1,
    SQUARE,
    CROSS,
    CIRCLE,
    TRIANGLE,
    L1,
    L2,
    L3,
    R1,
    R2,
    R3,
    SHARE,
    OPTIONS,
    PS,
    PAD,
    D_PAD_LEFT,
    D_PAD_RIGHT,
    D_PAD_UP,
    D_PAD_DOWN,
    LEFT_STICK_LEFT,
    LEFT_STICK_RIGHT,
    LEFT_STICK_UP,
    LEFT_STICK_DOWN,
    RIGHT_STICK_LEFT,
    RIGHT_STICK_RIGHT,
    RIGHT_STICK_UP,
    RIGHT_STICK_DOWN,
}

namespace Gamepad
{
    public class Playstation
    {
        static private KeyCode square = KeyCode.JoystickButton0;
        static private KeyCode cross = KeyCode.JoystickButton1;
        static private KeyCode circle = KeyCode.JoystickButton2;
        static private KeyCode triangle = KeyCode.JoystickButton3;
        static private KeyCode L1 = KeyCode.JoystickButton4;
        static private KeyCode R1 = KeyCode.JoystickButton5;
        static private KeyCode L2 = KeyCode.JoystickButton6;
        static private KeyCode R2 = KeyCode.JoystickButton7;
        static private KeyCode share = KeyCode.JoystickButton8;
        static private KeyCode options = KeyCode.JoystickButton9;
        static private KeyCode L3 = KeyCode.JoystickButton10;
        static private KeyCode R3 = KeyCode.JoystickButton11;
        static private KeyCode PS = KeyCode.JoystickButton12;
        static private KeyCode pad_press = KeyCode.JoystickButton13;
               
        static private string left_stick_x = "first_axis";
        static private string left_stick_y = "second_axis";
        static private string right_stick_x = "third_axis";
        static private string right_stick_y = "fourth_axis";
        static private string L2_axis = "fifth_axis";
        static private string R2_axis = "sixth_axis";
        static private string d_pad_x = "seventh_axis";
        static private string d_pad_y = "eigth_axis";

        static public bool Get_Button(PS_Button button)
        {
            switch (button)
            {
                case PS_Button.NONE: return false;
                case PS_Button.SQUARE: return Input.GetKey(square);
                case PS_Button.CROSS: return Input.GetKey(cross);
                case PS_Button.CIRCLE: return Input.GetKey(circle);
                case PS_Button.TRIANGLE: return Input.GetKey(triangle);
                case PS_Button.L1: return Input.GetKey(L1);
                case PS_Button.L2: return Input.GetKey(L2);
                case PS_Button.L3: return Input.GetKey(L3);
                case PS_Button.R1: return Input.GetKey(R1);
                case PS_Button.R2: return Input.GetKey(R2);
                case PS_Button.R3: return Input.GetKey(R3);
                case PS_Button.SHARE: return Input.GetKey(share);
                case PS_Button.OPTIONS: return Input.GetKey(options);
                case PS_Button.PS: return Input.GetKey(PS);
                case PS_Button.PAD: return Input.GetKey(pad_press);
                case PS_Button.D_PAD_LEFT: return Input.GetAxisRaw(d_pad_x) < 0;
                case PS_Button.D_PAD_RIGHT: return Input.GetAxisRaw(d_pad_x) > 0;
                case PS_Button.D_PAD_UP: return Input.GetAxisRaw(d_pad_y) > 0;
                case PS_Button.D_PAD_DOWN: return Input.GetAxisRaw(d_pad_y) < 0;
                case PS_Button.LEFT_STICK_LEFT: return Input.GetAxisRaw(left_stick_x) < 0;
                case PS_Button.LEFT_STICK_RIGHT: return Input.GetAxisRaw(left_stick_x) > 0;
                case PS_Button.LEFT_STICK_UP: return Input.GetAxisRaw(left_stick_y) < 0;
                case PS_Button.LEFT_STICK_DOWN: return Input.GetAxisRaw(left_stick_y) > 0;
                case PS_Button.RIGHT_STICK_LEFT: return Input.GetAxisRaw(right_stick_x) < 0;
                case PS_Button.RIGHT_STICK_RIGHT: return Input.GetAxisRaw(right_stick_x) > 0;
                case PS_Button.RIGHT_STICK_UP: return Input.GetAxisRaw(right_stick_y) > 0;
                case PS_Button.RIGHT_STICK_DOWN: return Input.GetAxisRaw(right_stick_y) < 0;
            }
            return false;
        }
    }
}