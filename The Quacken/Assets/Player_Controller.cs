using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player_Controller : MonoBehaviour
{
    private void Awake()
    {

    }

    void Start()
    {
        Service<Game_Manager>.Get().Set_Player(gameObject);
    }

    void Update()
    {
        if (Player_Input.Player(0).m_move_left)
            transform.position += Vector3.left * Time.deltaTime * 5.0f;
        if (Player_Input.Player(0).m_move_right)
            transform.position += Vector3.right * Time.deltaTime * 5.0f;
        if (Player_Input.Player(0).m_move_up)
            transform.position += Vector3.up * Time.deltaTime * 5.0f;
        if (Player_Input.Player(0).m_move_down)
            transform.position += Vector3.down * Time.deltaTime * 5.0f;
        //print(Player_Input.Player(0).m_current_device);
    }
}
