using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owl
{
    [CreateAssetMenu(menuName = "FSM States/Owl/Static Chase")]
    public class Chase_Back : Owl_State
    {
        private protected Physics2D_Movement m_movement;
        private protected Animator m_anim;
        private protected Rigidbody2D m_rb;

        public string m_if_reached_target;
        public string m_if_sees;

        public float m_cone_length_factor;

        public override void Init()
        {
            m_rb = m_context.GetComponent<Rigidbody2D>();
            m_movement = m_context.m_movement;
            m_anim = m_context.m_anim;
        }

        public override void Animate()
        {
            m_anim.SetFloat("move_x", m_direction.x);
            m_anim.SetFloat("move_y", m_direction.y);
            m_anim.SetFloat("idle_x", m_movement.prev_move_direction.x);
            m_anim.SetFloat("idle_y", m_movement.prev_move_direction.y);
        }


        public override void Enter()
        {
            m_context.Play_Notice();
            m_anim.SetBool("chase", true);
            m_context.m_seeing.m_cone_length *= m_cone_length_factor;
        }

        public override void Exit()
        {
            m_movement.Reset_Direction();
            m_anim.SetBool("chase", false);
            m_rb.velocity = Vector2.zero;
            m_context.m_seeing.m_cone_length /= m_cone_length_factor;
        }

        private protected float m_timer;
        private protected Vector2 m_direction;

        public override bool Run()
        {
            Vector2 position = m_context.transform.position;
            Vector2 other_position = m_context.m_player.transform.position;

            m_direction = (other_position - position);

            m_direction.Normalize();

            m_movement.Reset_Direction();
            m_movement.Add_Direction(m_direction);

            m_movement.Execute();

            if (!m_context.m_sees)
            {
                m_next = m_if_reached_target;
                return false;
            }

            return true;
        }
    }
}
