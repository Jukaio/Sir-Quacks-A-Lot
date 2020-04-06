using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player_Controller : MonoBehaviour
{
    public float m_noise_range = 3.0f;

    private void Awake()
    {

    }

    void Start()
    {
        Service<Game_Manager>.Get().Set_Player(gameObject);
    }

    void Update()
    {
        if (Player_Input.Player(0).Move_Left)
            transform.position += Vector3.left * Time.deltaTime * 5.0f;
        if (Player_Input.Player(0).Move_Right)
            transform.position += Vector3.right * Time.deltaTime * 5.0f;
        if (Player_Input.Player(0).Move_Up)
            transform.position += Vector3.up * Time.deltaTime * 5.0f;
        if (Player_Input.Player(0).Move_Down)
            transform.position += Vector3.down * Time.deltaTime * 5.0f;
        //print(Player_Input.Player(0).m_current_device);
    }
}
