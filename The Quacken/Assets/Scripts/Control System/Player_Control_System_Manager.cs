using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the amount of players and creates an instance of a player system for each player
// This value is static for now

public class Player_Control_System_Manager : MonoBehaviour
{
    public int m_player_amount;
    public GameObject m_player_system_template;


    void Awake()
    {
        for(int index = 0; index < m_player_amount; index++)
        {
            Instantiate(m_player_system_template, transform).name = "Player_System " + index;
        }
    }

    void Update()
    {
        
    }
}
