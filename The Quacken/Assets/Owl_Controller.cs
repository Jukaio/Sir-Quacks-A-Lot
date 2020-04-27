using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Owl_Controller : Enemy_Base
{
    public GameObject[] m_waypoints;
    int m_index = 0;
    bool m_bounce = false;

    public enum Waypoint_Type
    {
        LOOP,
        BOUNCE,
        ONCE
    }
    public Waypoint_Type m_type;

    private enum State
    {
        ENTER_MOVEMENT,
        MOVEMENT,
        LOOK_AROUND,
    }
    State m_state;

    Vector3 m_look_at;
    Vector3 m_sit_direction;
    float m_timer;
    int m_rotations;
    public override void Behaviour()
    {
        switch (m_state)
        {
            case State.ENTER_MOVEMENT:
                m_movement.Enter_Movement(m_waypoints[m_index].transform.position);
                m_movement.Set_Speed(1.0f);
                m_state = State.MOVEMENT;
                break;

            case State.MOVEMENT:
                if (m_movement.Movement())
                {
                    Next_Target();
                    m_movement.Set_Speed(0.0f);
                    m_state = State.LOOK_AROUND;
                    m_rotations = 0;
                    m_timer = 0.4f;
                    m_look_at = -m_movement.prev_direction;
                    m_sit_direction = m_look_at;
                    m_anim.SetFloat("x", 0.0f);
                    m_anim.SetFloat("y", 0.0f);
                    if (Math.Atan2(m_look_at.x, m_look_at.y) < Math.Atan2((Vector2.up + Vector2.right).x, (Vector2.up + Vector2.right).y) &&
                        Math.Atan2(m_look_at.x, m_look_at.y) > Math.Atan2((Vector2.up + Vector2.left).x, (Vector2.up + Vector2.left).y))
                        m_look_at = Vector2.up;
                    else if (Math.Atan2(m_look_at.x, m_look_at.y) < Math.Atan2((Vector2.right + Vector2.down).x, (Vector2.right + Vector2.down).y) &&
                             Math.Atan2(m_look_at.x, m_look_at.y) > Math.Atan2((Vector2.right + Vector2.up).x, (Vector2.right + Vector2.up).y))
                        m_look_at = Vector2.right;
                    else if (Math.Atan2(m_look_at.x, m_look_at.y) < Math.Atan2((Vector2.down + Vector2.left).x, (Vector2.down + Vector2.left).y) &&
                             Math.Atan2(m_look_at.x, m_look_at.y) > Math.Atan2((Vector2.down + Vector2.right).x, (Vector2.down + Vector2.right).y))
                        m_look_at = Vector2.down;
                    else if (Math.Atan2(m_look_at.x, m_look_at.y) < Math.Atan2((Vector2.left + Vector2.up).x, (Vector2.left + Vector2.up).y) &&
                             Math.Atan2(m_look_at.x, m_look_at.y) > Math.Atan2((Vector2.left + Vector2.down).x, (Vector2.left + Vector2.down).y))
                        m_look_at = Vector2.left;
                }
                break;

            case State.LOOK_AROUND:
                m_movement.Add_Direction(m_look_at);
                m_timer -= Time.deltaTime;
                if (m_rotations > 18)
                    m_state = State.ENTER_MOVEMENT;
                if (m_timer <= 0.0f)
                {
                    m_look_at = m_look_at.Rotate(20.0f);
                    m_movement.Add_Direction(m_look_at.normalized);
                    m_timer = 0.4f;
                    m_rotations++;
                }

                break;
            default:
                break;
        }

        m_movement.Normalise();

        if (m_seeing.Sense(m_movement.direction, m_player))
            ;
        if (m_hearing.Sense(m_player.GetComponent<Player_Controller>().m_noise_range, m_player))
            Debug.LogWarning("I hear");
    }

    void Next_Target()
    {
        switch (m_type)
        {
            case Waypoint_Type.LOOP:
                m_index++;
                if (m_index >= m_waypoints.Length)
                {
                    m_index = 0;
                }
                break;
            case Waypoint_Type.BOUNCE:
                if (m_index >= m_waypoints.Length - 1 && !m_bounce)
                {
                    m_bounce = true;
                    m_index--;
                }
                else if (m_index <= 0 && m_bounce)
                {
                    m_bounce = false;
                    m_index++;
                }
                else if (!m_bounce)
                    m_index++;
                else if (m_bounce)
                    m_index--;

                break;
            case Waypoint_Type.ONCE:
                m_index++;
                if (m_index >= m_waypoints.Length)
                {
                    m_index = m_waypoints.Length - 1;
                }
                break;
            default:
                break;
        }
    }

    public override void Animate()
    {
        if (m_state != State.LOOK_AROUND)
        {
            m_anim.SetFloat("x", m_movement.direction.x);
            m_anim.SetFloat("y", m_movement.direction.y);
            m_anim.SetFloat("prev_x", m_movement.prev_direction.x);
            m_anim.SetFloat("prev_y", m_movement.prev_direction.y);
        }
        else
        {
            m_anim.SetFloat("prev_x", m_sit_direction.x);
            m_anim.SetFloat("prev_y", m_sit_direction.y);
            m_anim.SetFloat("head_x", m_look_at.x);
            m_anim.SetFloat("head_y", m_look_at.y);
        }
    }
}
