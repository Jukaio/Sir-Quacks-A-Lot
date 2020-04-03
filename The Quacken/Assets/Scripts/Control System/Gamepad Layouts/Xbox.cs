using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control layout for xbox
namespace Gamepad_Layouts
{
    public class Xbox
    {
        static public KeyCode A = KeyCode.JoystickButton0;
        static public KeyCode B = KeyCode.JoystickButton1;
        static public KeyCode X = KeyCode.JoystickButton2;
        static public KeyCode Y = KeyCode.JoystickButton3;
        static public KeyCode left_bumper = KeyCode.JoystickButton4;
        static public KeyCode right_bumper = KeyCode.JoystickButton5;
        static public KeyCode back = KeyCode.JoystickButton6;
        static public KeyCode start = KeyCode.JoystickButton7;
        static public KeyCode left_stick_button = KeyCode.JoystickButton8;
        static public KeyCode right_stick_button = KeyCode.JoystickButton9;

        static public string left_stick_x = "X-Axis";
        static public string left_stick_y = "Y-Axis";
        static public string right_stick_x = "4rd Axis";
        static public string right_stick_y = "5th Axis";
        static public string L2_axis = "9th Axis";
        static public string R2_axis = "10th Axis";
        static public string d_pad_x = "6th Axis";
        static public string d_pad_y = "7th Axis";
    }
}