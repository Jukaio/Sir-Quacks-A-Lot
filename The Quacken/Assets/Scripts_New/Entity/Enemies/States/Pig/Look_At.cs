using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM States/Pig/Look At")]
public class Look_At : Pig_State
{
    Physics2D_Movement m_movement;
    Animator m_anim;

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

    }

    public override void Exit()
    {
        m_context.Next_Target();
    }

    public override bool Run()
    {
        if (m_context.m_seeing.Sense(m_movement.view_direction, m_context.m_player))
        {
            if(m_context.m_seeing.full_Feedback)
            {
                Debug.Log("CHASE!!");
            }

            return true;
        }
        else
        {
            m_next = m_context.m_state_machine.m_previous.m_name;
            return false;
        }
    }
}
