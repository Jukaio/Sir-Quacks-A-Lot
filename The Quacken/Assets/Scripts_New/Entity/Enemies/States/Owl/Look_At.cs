using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owl
{
    [CreateAssetMenu(menuName = "FSM States/Owl/Look at")]
    public class Look_At : Owl_State
    {
        private protected Physics2D_Movement m_movement;
        private protected Animator m_anim;

        public string m_if_fully_seen;

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
            
        }

        public override bool Run()
        {
            if (m_context.m_sees)
            {
                if (m_context.m_seeing.full_Feedback)
                {
                    m_next = m_if_fully_seen;
                    return false;
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
}