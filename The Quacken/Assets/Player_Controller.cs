using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Player_Controller : MonoBehaviour
{
    public float m_noise_range = 3.0f;
    public CompositeCollider2D m_objects_composite_collider;

    Vector2[][] paths_points;
    public Player_Input m_input;
    private void Awake()
    {

    }

    void Start()
    {
        Service<Game_Manager>.Get().Set_Player(gameObject);
        m_input = Player_Input.Player(0);
        Set_Ray_Data();
    }

    void Update_Noise_Range()
    {
        transform.GetChild(1).transform.localScale = (Vector3.one / 10.0f) * m_noise_range;

    }

    void Update()
    {
        Update_Noise_Range();

        if (m_input.Move_Left)
            transform.position += Vector3.left * Time.deltaTime * 5.0f;
        if (m_input.Move_Right)
            transform.position += Vector3.right * Time.deltaTime * 5.0f;
        if (m_input.Move_Up)
            transform.position += Vector3.up * Time.deltaTime * 5.0f;
        if (m_input.Move_Down)
            transform.position += Vector3.down * Time.deltaTime * 5.0f;
        ////print(Player_Input.Player(0).m_current_device);

        Set_Ray_Data();
        Debug_Draw_Rays();
    }

    void Set_Ray_Data()
    {
        paths_points = new Vector2[m_objects_composite_collider.pathCount][];

        for (int i = 0; i < m_objects_composite_collider.pathCount; i++)
        {
            paths_points[i] = new Vector2[m_objects_composite_collider.GetPathPointCount(i)];
            int nrOfPoints = m_objects_composite_collider.GetPath(i, paths_points[i]);
        }
        Debug.Log(m_objects_composite_collider.pointCount);
    }

    void Debug_Draw_Rays()
    {
        for (int arr_i = 0; arr_i < paths_points.Length; arr_i++)
        {
            for (int jag_i = 0; jag_i < paths_points[arr_i].Length; jag_i++)
            {
                Debug.DrawLine(transform.position, paths_points[arr_i][jag_i], Color.green);
            }
        }
    }
}
