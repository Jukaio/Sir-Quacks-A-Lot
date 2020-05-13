using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : MonoBehaviour
{
    private GameObject m_player;

    int size = 64;
    int height = 4;
    int width = 4;

    CompositeCollider2D[,] levels = new CompositeCollider2D[0, 0];

    private Vector2Int m_current_index;
    private Vector2Int m_previous_index = new Vector2Int(-1, -1);


    void Start()
    {
        levels = new CompositeCollider2D[width, height];
        m_player = Service<Game_Manager>.Get().Player;

        for (int i = 0; i < transform.childCount; i++)
        {
            CompositeCollider2D temp;
            if (transform.GetChild(i).gameObject.TryGetComponent<CompositeCollider2D>(out temp))
            {
                Vector3Int grid_pos = Vector3Int.FloorToInt((temp.transform.position / size));
                levels[grid_pos.x, (int)Mathf.Abs(grid_pos.y)] = temp;
                if (temp.gameObject.activeSelf)
                    temp.gameObject.SetActive(false);
            }
        }
        Set_Level();
    }
    void Update()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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

        Set_Level();
    }

    void Set_Level()
    {
        m_current_index = new Vector2Int((int)(m_player.transform.position.x / size), (int)(Mathf.Abs(m_player.transform.position.y) / size));
        if (m_current_index != m_previous_index)
        {
            if (m_previous_index != new Vector2Int(-1, -1))
                levels[m_previous_index.x, m_previous_index.y].gameObject.SetActive(false);
            var temp = levels[m_current_index.x, m_current_index.y];
            temp.gameObject.SetActive(true);

            m_player.GetComponent<Shadow>().Set_Map(temp);
        }
        m_previous_index = m_current_index;
    }
}
