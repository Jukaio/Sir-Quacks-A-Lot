using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pig_Controller : Enemy_Base
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

    enum Movement_States
    {
        UP, DOWN, LEFT, RIGHT, CHASE,
        ENTER_MOVEMENT,
        MOVEMENT,
        LOOK_AROUND,
    }
    Movement_States m_state;


    public bool m_use_waypoints;

    public override void Behaviour()
    {
        if (m_use_waypoints)
            State_Behaviour();
        else
            Ray_Cast_Behaviour();

        if (m_seeing.Sense(m_movement.direction, m_player))
            m_state = Movement_States.CHASE;
        if (m_hearing.Sense(m_player.GetComponent<Player_Controller>().m_noise_range, m_player))
            Debug.LogWarning("I hear");
    }

    void Ray_Cast_Behaviour()
    {
        switch (m_state)
        {
            case Movement_States.UP:
                m_movement.Add_Direction(Vector2.up);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity).centroid) < 1.25f)
                    m_state = Movement_States.LEFT;
                break;
            case Movement_States.DOWN:
                m_movement.Add_Direction(Vector2.down);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity).centroid) < 1.25f)
                    m_state = Movement_States.RIGHT;
                break;
            case Movement_States.LEFT:
                m_movement.Add_Direction(Vector2.left);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity).centroid) < 1.5f)
                    m_state = Movement_States.DOWN;
                break;
            case Movement_States.RIGHT:
                m_movement.Add_Direction(Vector2.right);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity).centroid) < 1.5f)
                    m_state = Movement_States.UP;
                break;

            case Movement_States.CHASE:
                m_movement.Add_Direction((m_player.transform.position - transform.position).normalized);
                if (!m_seeing.Sense(m_movement.direction, m_player))
                    m_state = Movement_States.UP;
                break;

            default:
                m_state = Movement_States.UP;
                break;
        }
    }

    void State_Behaviour()
    {
        switch (m_state)
        {
            case Movement_States.ENTER_MOVEMENT:
                m_movement.Enter_Movement(m_waypoints[m_index].transform.position);
                m_movement.Set_Speed(1.0f);
                m_state = Movement_States.MOVEMENT;
                break;

            case Movement_States.MOVEMENT:
                if (m_movement.Movement())
                {
                    Next_Target();
                    m_state = Movement_States.ENTER_MOVEMENT;
                }
                break;


            default:
                m_state = Movement_States.ENTER_MOVEMENT;
                break;
        }
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
        m_anim.SetFloat("x", m_movement.direction.x);
        m_anim.SetFloat("y", m_movement.direction.y);
        m_anim.SetFloat("prev_x", m_movement.prev_direction.x);
        m_anim.SetFloat("prev_y", m_movement.prev_direction.y);
    }
}
