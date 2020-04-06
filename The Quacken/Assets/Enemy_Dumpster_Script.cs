using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dumpster_Script : MonoBehaviour
{
    // To player
    private GameObject m_player;
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;

    // From this (Enemy)
    Vector2 m_origin = Vector2.up;
    Vector2 m_view_direction;
    float m_view_angle;

    [SerializeField] private float m_cone_width;
    [SerializeField] private float m_cone_length;


    // Interface Visuals for early development
    public GameObject point; // For debugging
    bool sees; //For debugging


    void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
    }


    void Update()
    {
        sees = See_Player();
    }

    bool See_Player()
    {
        m_view_direction = m_origin.Rotate(transform.rotation.eulerAngles.z);
        m_to_player_direction = m_player.transform.position - transform.position;
        m_to_player_distance = Vector2.Distance(transform.position, m_player.transform.position);

        m_view_angle = Vector2.Angle(m_origin, m_view_direction);
        m_to_player_angle = Vector2.Angle(m_origin, m_to_player_direction.normalized);

        point.transform.position = transform.position + (Vector3)m_view_direction;

        return  m_to_player_angle > m_view_angle - m_cone_width && m_to_player_angle < m_view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }
}
