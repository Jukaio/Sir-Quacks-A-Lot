using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Owl_Controller : Enemy_Base
{
    [SerializeField] Owl_State_Data m_state_machine_data;
    public State_Machine<Owl_Controller> m_state_machine = new State_Machine<Owl_Controller>();
    public GameObject[] m_waypoints;
    public int m_prev_index = 0;
    public int m_index = 0;
    bool m_bounce = false;
    public enum Waypoint_Type
    {
        LOOP,
        BOUNCE,
        ONCE
    }
    public Waypoint_Type m_type;

    public string current;

    public override void Init()
    {
        foreach (var state in m_state_machine_data.m_states)
        {
            var temp = ScriptableObject.Instantiate(state);
            temp.Init(this);
            m_state_machine.Add(state.m_name, temp);
        }
        m_state_machine.Set(m_state_machine_data.m_states[0].m_name);
    }

    public override void Behaviour()
    {
        m_state_machine.Run();
        current = m_state_machine.m_current.m_name;
    }

    public void Next_Target()
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

    //public override void Animate()
    //{
    //    if (m_state != State.LOOK_AROUND)
    //    {
    //        m_anim.SetFloat("x", m_movement.move_direction.x);
    //        m_anim.SetFloat("y", m_movement.move_direction.y);
    //        m_anim.SetFloat("prev_x", m_movement.prev_move_direction.x);
    //        m_anim.SetFloat("prev_y", m_movement.prev_move_direction.y);
    //    }
    //    else
    //    {
    //        m_anim.SetFloat("prev_x", m_sit_direction.x);
    //        m_anim.SetFloat("prev_y", m_sit_direction.y);
    //        m_anim.SetFloat("head_x", m_look_at.x);
    //        m_anim.SetFloat("head_y", m_look_at.y);
    //    }
    //}
}
