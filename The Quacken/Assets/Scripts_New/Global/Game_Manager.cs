using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private GameObject m_player;
    [SerializeField] private Sound_Manager m_sound_manager; // Global Managers get assigned before the game starts
    [SerializeField] private Map_Manager m_level_manager;
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
        Service<Sound_Manager>.Set(m_sound_manager);
        Service<Map_Manager>.Set(m_level_manager);
        //Scene_Manager.Load_Level(1);
    }

    public void Set_Player(GameObject p_player)
    {
        m_player = p_player;
    }

    private void Update()
    {
        
    }
}
