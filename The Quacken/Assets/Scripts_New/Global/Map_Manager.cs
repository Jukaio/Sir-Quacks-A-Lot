using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    private GameObject m_player;
    private Map m_current_map;
    public Map Map
    {
        get => m_current_map;
        set => m_current_map = value;
    }


    void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
    }

    void Update()
    {
        int size = 64;
        for(int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Vector2 top_left = new Vector2(x * size, -y * size);
                Vector2 top_right = new Vector2((x + 1) * size, -y * size);
                Vector2 bottom_right = new Vector2((x + 1) * size, -(y + 1) * size);
                Vector2 bottom_left = new Vector2(x * size, -(y + 1) * size);

                Debug.DrawLine(top_left, top_right, Color.red);
                Debug.DrawLine(top_right, bottom_right, Color.red);
                Debug.DrawLine(bottom_right, bottom_left, Color.red);
                Debug.DrawLine(bottom_left, top_left, Color.red);

            }
        }
    }
}
