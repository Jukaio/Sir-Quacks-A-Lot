using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM States/Pig/Move")]
public class Move : Pig_State
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
        m_anim.SetFloat("move_x", m_movement.move_direction.x);
        m_anim.SetFloat("move_y", m_movement.move_direction.y);
        m_anim.SetFloat("idle_x", m_movement.prev_move_direction.x);
        m_anim.SetFloat("idle_y", m_movement.prev_move_direction.y);
    }

    public override void Enter()
    {
        m_movement.Enter_Move(m_context.m_waypoints[m_context.m_index].transform.position);
        m_movement.Set_Speed(m_movement.Initial_Speed);
    }

    public override void Exit()
    {
            m_context.Next_Target();
    }

    public override bool Run()
    {
        if (m_movement.Move())
        {
            m_next = m_if_reached_target;
            return false;
        }

        if (m_context.m_sees)
        {
            m_next = m_if_sees;
            return false;
        }
        return true;
    }
}

