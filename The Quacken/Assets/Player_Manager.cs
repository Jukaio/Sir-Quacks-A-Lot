using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public int m_player_amount;
    public GameObject m_player_system_template;

    void Start()
    {
        for(int index = 0; index < m_player_amount; index++)
        {
            Instantiate<GameObject>(m_player_system_template, transform).name = "Player_System " + index;
        }
    }

    void Update()
    {
        
    }
}
