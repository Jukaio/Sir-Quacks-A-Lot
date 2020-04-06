using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Control layout for xbox
public enum XBOX_Button
{
    NONE = -1,
    A,
    B,
    X,
    Y,
    LEFT_BUMPER,
    LEFT_TRIGGER,
    LEFT_STICK_BUTTON,
    RIGHT_BUMPER,
    RIGHT_TRIGGER,
    RIGHT_STICK_BUTTON,
    BACK,
    START,
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

        static public string left_stick_x = "first_axis";
        static public string left_stick_y = "second_axis";
        static public string right_stick_x = "fourth_axis";
        static public string right_stick_y = "fifth_axis";
        static public string L2_axis = "ninth_axis";
        static public string R2_axis = "tenth_axis";
        static public string d_pad_x = "sixth_axis";
        static public string d_pad_y = "seventh_axis";

        static public bool Get_Button(XBOX_Button button)
        {
            switch (button)
            {
                case XBOX_Button.NONE: return false;
                case XBOX_Button.X: return Input.GetKey(X);
                case XBOX_Button.A: return Input.GetKey(A);
                case XBOX_Button.Y: return Input.GetKey(Y);
                case XBOX_Button.B: return Input.GetKey(B);
                case XBOX_Button.LEFT_BUMPER: return Input.GetKey(left_bumper);
                case XBOX_Button.LEFT_TRIGGER: return Input.GetAxisRaw(L2_axis) > 0.0f;
                case XBOX_Button.LEFT_STICK_BUTTON: return Input.GetKey(left_stick_button);
                case XBOX_Button.RIGHT_BUMPER: return Input.GetKey(right_bumper);
                case XBOX_Button.RIGHT_TRIGGER: return Input.GetAxisRaw(R2_axis) > 0.0f;
                case XBOX_Button.RIGHT_STICK_BUTTON: return Input.GetKey(right_stick_button);
                case XBOX_Button.BACK: return Input.GetKey(back);
                case XBOX_Button.START: return Input.GetKey(start);
                case XBOX_Button.D_PAD_LEFT: return Input.GetAxisRaw(d_pad_x) < 0;
                case XBOX_Button.D_PAD_RIGHT: return Input.GetAxisRaw(d_pad_x) > 0;
                case XBOX_Button.D_PAD_UP: return Input.GetAxisRaw(d_pad_y) > 0;
                case XBOX_Button.D_PAD_DOWN: return Input.GetAxisRaw(d_pad_y) < 0;
                case XBOX_Button.LEFT_STICK_LEFT: return Input.GetAxisRaw(left_stick_x) < 0;
                case XBOX_Button.LEFT_STICK_RIGHT: return Input.GetAxisRaw(left_stick_x) > 0;
                case XBOX_Button.LEFT_STICK_UP: return Input.GetAxisRaw(left_stick_y) < 0;
                case XBOX_Button.LEFT_STICK_DOWN: return Input.GetAxisRaw(left_stick_y) > 0;
                case XBOX_Button.RIGHT_STICK_LEFT: return Input.GetAxisRaw(right_stick_x) < 0;
                case XBOX_Button.RIGHT_STICK_RIGHT: return Input.GetAxisRaw(right_stick_x) > 0;
                case XBOX_Button.RIGHT_STICK_UP: return Input.GetAxisRaw(right_stick_y) > 0;
                case XBOX_Button.RIGHT_STICK_DOWN: return Input.GetAxisRaw(right_stick_y) < 0;
            }
            return false;
        }
    }
}