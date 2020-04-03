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

        print(Player_Inputs.Player(0).m_move_left);
        print(Input.GetAxisRaw("Horizontal"));
        if (Player_Inputs.Player(0).m_move_left)
            transform.position += Vector3.left;
    }
}
