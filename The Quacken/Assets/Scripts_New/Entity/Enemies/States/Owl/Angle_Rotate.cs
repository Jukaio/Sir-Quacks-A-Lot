using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owl
{
    [CreateAssetMenu(menuName = "FSM States/Owl/Rotate by Angle")]
    public class Angle_Rotate : Owl_State
    {
        Physics2D_Movement m_movement;
        Animator m_anim;

        public string m_if_reached_target;
        public string m_if_sees;

        public float m_angle;

        public override void Init()
        {
            m_movement = m_context.m_movement;
            m_anim = m_context.m_anim;
        }

        public override void Animate()
        {
            m_anim.SetFloat("move_x", 0.0f);
            m_anim.SetFloat("move_y", 0.0f);

            float angle = Mathf.Rad2Deg * (float)Utility.Angle.Angle_Between_Segments(Vector2.left, m_movement.view_direction, Vector2.zero);
            angle = Mathf.Abs(angle - 360.0f);
            m_anim.SetFloat("eyes_angle", angle);
            m_anim.SetFloat("idle_x", m_movement.prev_move_direction.x);
            m_anim.SetFloat("idle_y", m_movement.prev_move_direction.y);
        }

        public override void Enter()
        {
            m_movement.Enter_Rotation(m_angle);
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

            if (m_context.m_sees)
            {
                m_next = m_if_sees;
                return false;
            }

            return true;
        }
    }
}