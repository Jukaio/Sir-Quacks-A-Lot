using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dumpster_Script : MonoBehaviour
{
    ///////////////////////
    /// SerializeFields ///
    ///////////////////////

    // Seeing
    [SerializeField] private float m_cone_width = 30.0f;
    [SerializeField] private float m_cone_length = 5.0f;
    // Hearing
    [SerializeField] private float m_hearing_range = 3.0f;

    ////////////////////////
    /// !SerializeFields ///
    ////////////////////////

    // To player
    // Seeing
    private GameObject m_player;
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;

    // From this (Enemy)
    Vector2 m_origin = Vector2.up;
    Vector2 m_view_direction;
    float m_view_angle;

    // Interface Visuals for early development
    public GameObject point; // For debugging
    bool sees; // For debugging
    bool hears; // For debugging

    void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
    }


    void Update()
    {
        Set_Distance_To_Player();
        sees = Sees_Player();
        hears = Hears_Player();

    }

    void Set_Distance_To_Player()
    {
        m_to_player_direction = m_player.transform.position - transform.position;
        m_to_player_distance = Vector2.Distance(transform.position, m_player.transform.position);
    }

    bool Sees_Player()
    {
        m_view_direction = m_origin.Rotate(transform.rotation.eulerAngles.z);
        m_view_angle = Vector2.Angle(m_origin, m_view_direction);
        m_to_player_angle = Vector2.Angle(m_origin, m_to_player_direction.normalized);

        point.transform.position = transform.position + (Vector3)m_view_direction;

        return  m_to_player_angle > m_view_angle - m_cone_width && m_to_player_angle < m_view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }

    bool Hears_Player()
    {
        return m_to_player_distance < m_player.GetComponent<Player_Controller>().m_noise_range + m_hearing_range;
    }
}
