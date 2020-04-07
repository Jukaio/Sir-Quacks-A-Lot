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
    [SerializeField] private string m_tag_to_compare = "Player";
    // Hearing
    [SerializeField] private float m_hearing_range = 3.0f;

    ////////////////////////
    /// !SerializeFields ///
    ////////////////////////

    // to_player
    // Seeing
    // Cone
    private GameObject m_player;
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;
    // Raycast - If player is behind a collider or not
    RaycastHit2D m_hit = new RaycastHit2D();

    // hearing
    float m_player_noise_range;
    // !to_player

    // this (Enemy)
    Vector2 m_origin = Vector2.up;
    Vector2 m_view_direction;
    float m_view_angle;
    // !this

    // Interface Visuals for early development
    bool sees; // For debugging
    bool hears; // For debugging

    void Start()
    {
        m_player = Service<Game_Manager>.Get().Player;
        m_player_noise_range = m_player.GetComponent<Player_Controller>().m_noise_range;
    }


    void Update()
    {
        Set_Distance_To_Player();
        sees = Sees_Player();
        hears = Hears_Player();

    }

    void Set_Distance_To_Player()
    {
        m_to_player_direction = (m_player.transform.position - transform.position).normalized;
        m_to_player_distance = Vector2.Distance(transform.position, m_player.transform.position);
    }

    bool Player_In_Vision_Range()
    {
        m_view_direction = m_origin.Rotate(transform.rotation.eulerAngles.z);
        m_view_angle = Vector2.Angle(m_origin, m_view_direction);
        m_to_player_angle = Vector2.Angle(m_origin, m_to_player_direction.normalized);

        return  m_to_player_angle > m_view_angle - m_cone_width && m_to_player_angle < m_view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }

    bool Sees_Player()
    {
        if (Player_In_Vision_Range())
        {
            m_hit = Physics2D.Raycast(transform.position, m_to_player_direction);
            if (m_hit.collider.CompareTag(m_tag_to_compare))
                return true;
        }
        return false;
    }

    bool Hears_Player()
    {
        return m_to_player_distance < m_player_noise_range + m_hearing_range;
    }
}
