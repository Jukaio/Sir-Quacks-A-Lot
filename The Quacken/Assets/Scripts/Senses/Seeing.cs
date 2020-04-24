using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeing : Sensing
{
    [SerializeField] private float m_cone_width;
    [SerializeField] private float m_cone_length;

    //// Internal variables
    // Player

    // to_player calculations
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;
    RaycastHit2D m_to_player_ray_hit = new RaycastHit2D();
    // !to_player calculations

    void Start()
    {

    }


    bool Player_In_Vision_Range(Vector2 p_view_direction)
    {
        float view_angle = Mathf.Atan2(p_view_direction.x, p_view_direction.y);

        m_to_player_angle = Vector2.Angle(p_view_direction.normalized, m_to_player_direction.normalized);

        Vector2 look_direction = (Vector2)(p_view_direction * m_cone_length);
        Debug.DrawLine(transform.position, (Vector2)transform.position + look_direction.Rotate(-m_cone_width), Color.red);
        Debug.DrawLine(transform.position, (Vector2)transform.position + look_direction.Rotate(m_cone_width), Color.red);
        Debug.DrawLine((Vector2)transform.position + look_direction.Rotate(-m_cone_width), (Vector2)transform.position + look_direction, Color.red);
        Debug.DrawLine((Vector2)transform.position + look_direction, (Vector2)transform.position + look_direction.Rotate(m_cone_width), Color.red);

        return m_to_player_angle > view_angle - m_cone_width && m_to_player_angle < view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }

    public bool Sense(Vector2 p_view_direction, GameObject p_target)
    {
        m_to_player_direction = Set_Direction_To_Target(p_target);
        m_to_player_distance = Set_Distance_To_Target(p_target);

        if (Player_In_Vision_Range(p_view_direction))
        {
            m_to_player_ray_hit = Physics2D.Raycast(transform.position, m_to_player_direction * m_to_player_distance);
            if (m_to_player_ray_hit.collider.CompareTag(p_target.tag))
                return true;
        }
        return false;
    }
}
