using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player_Controller : MonoBehaviour
{
    
    void Start()
    {

    }

    void Update()
    {
        if (Player_Input.Player(0).m_move_left)
            print("Left");
        if (Player_Input.Player(0).m_move_right)
            print("Right");
        if (Player_Input.Player(0).m_move_up)
            print("Up");
        if (Player_Input.Player(0).m_move_down)
            print("Down");
        print(Player_Input.Player(0).m_current_device);
    }
}
