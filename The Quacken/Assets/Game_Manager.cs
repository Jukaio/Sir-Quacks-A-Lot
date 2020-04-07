using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private GameObject m_player;
    public GameObject Player
    {
        get
        {
            return m_player;
        }
        private set
        {

        }
    }

    Game_Manager()
    {
        Service<Game_Manager>.Set(this);
    }

    private void Awake()
    {

    }

    public void Set_Player(GameObject p_player)
    {
        m_player = p_player;
    }

    private void Update()
    {

    }
}
