using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Pig_Controller : Enemy_Base
{
    [SerializeField] Pig_State_Data m_state_machine_data;
    public State_Machine<Pig_Controller> m_state_machine = new State_Machine<Pig_Controller>();
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


    public bool m_use_waypoints;

    public override void Init()
    {
        foreach(var state in m_state_machine_data.m_states)
        {
            state.Init(this);
            m_state_machine.Add(state.m_name, state);
        }
        m_state_machine.Set(m_state_machine_data.m_states[0].m_name);
    }

    public override void Behaviour()
    {
        m_state_machine.Run();
    }


    public void Next_Target()
    {
        m_prev_index = m_index;
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
