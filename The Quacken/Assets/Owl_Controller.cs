using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        MOVEMENT
    }
    State m_state;

    public override void Behaviour()
    {
        switch (m_state)
        {
            case State.ENTER_MOVEMENT:
                m_movement.Enter_Movement(m_waypoints[m_index].transform.position);
                m_state = State.MOVEMENT;
                break;

            case State.MOVEMENT:
                if(m_movement.Movement())
                {
                    Next_Target();
                    m_state = State.ENTER_MOVEMENT;
                }
                break;
            default:
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

}
