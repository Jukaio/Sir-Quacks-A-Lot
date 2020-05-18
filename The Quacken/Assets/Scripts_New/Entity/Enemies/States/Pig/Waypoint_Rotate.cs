using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM States/Pig/Rotate to Waypoint")]
public class Waypoint_Rotate : Pig_State
{
    Physics2D_Movement m_movement;
    Animator m_anim;

    public string m_if_reached_target;
    public string m_if_sees;

    public override void Init()
    {
        m_movement = m_context.m_movement;
        m_anim = m_context.m_anim;
    }

    public override void Animate()
    {
        m_anim.SetFloat("move_x", 0.0f);
        m_anim.SetFloat("move_y", 0.0f);
        m_anim.SetFloat("idle_x", m_movement.view_direction.x);
        m_anim.SetFloat("idle_y", m_movement.view_direction.y);
    }

    public override void Enter()
    {
        Vector2 to_next = m_context.m_waypoints[m_context.m_index].transform.position - m_context.transform.position;

        float to_rotate = Mathf.Rad2Deg * (float)Utility.Angle.Angle_Between_Segments(to_next, m_movement.view_direction, Vector2.zero); // view direction is a child 
        
        if(to_rotate > 180)
        {
            to_rotate = to_rotate - 360;
        }
        
        m_movement.Enter_Rotation(to_rotate);
    }

    public override void Exit()
    {

    }

    public override bool Run()
    {
        if (m_movement.Rotate())
        {
            m_next = m_if_reached_target;
            return false;
        }

        if (m_context.m_seeing.Sense(m_movement.view_direction, m_context.m_player))
        {
            m_next = m_if_sees;
            return false;
        }

        return true;
    }
}