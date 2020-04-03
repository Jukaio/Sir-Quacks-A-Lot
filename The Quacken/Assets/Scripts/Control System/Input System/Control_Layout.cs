using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class All_Controls
{
    public Controls_Keyboard Keyboard;
    public Controls_Controller Playstation;
    public Controls_Controller Xbox;
}
