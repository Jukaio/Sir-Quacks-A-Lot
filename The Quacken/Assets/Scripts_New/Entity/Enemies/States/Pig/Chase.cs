using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM States/Pig/Chase")]
public class Chase : Pig_State
{
    Physics2D_Movement m_movement;
    Animator m_anim;
    Rigidbody2D m_rb;

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
        //m_movement.Enter_Move(m_context.m_player.transform.position);
        //m_movement.Set_Speed(m_movement.Initial_Speed);
        m_context.m_seeing.m_cone_length *= m_cone_length_factor;
    }

    public override void Exit()
    {
        m_anim.SetBool("chase", false);
        m_movement.Reset_Direction();
        m_rb.velocity = Vector2.zero;
        m_context.m_seeing.m_cone_length /= m_cone_length_factor;
    }

    private float m_timer;
    Vector2 m_direction;

    public override bool Run()
    {
        Vector2 position = m_context.transform.position;
        Vector2 other_position = m_context.m_player.transform.position;

        m_direction = (other_position - position);

        m_direction.Normalize();

        m_movement.Reset_Direction();
        m_movement.Add_Direction(m_direction);

        m_movement.Execute();

        if(!m_context.m_sees)
        {
            SortedList<float, int> temp_list = new SortedList<float, int>();
            for (int i = 0; i < m_context.m_waypoints.Length; i++)
            {
                temp_list.Add(Mathf.Pow(m_context.m_waypoints[i].transform.position.x - m_context.transform.position.x, 2) + Mathf.Pow(m_context.m_waypoints[i].transform.position.y - m_context.transform.position.y, 2), i);
            }
            m_context.m_index = temp_list.Values[0];
            m_next = m_if_reached_target;
            return false;
        }

        //if (m_movement.Move())
        //{
        //    if (m_context.m_sees)
        //    {
        //        m_movement.Enter_Move(m_context.m_player.transform.position);
        //        return true;
        //    }
        //    else
        //    {
        //        SortedList<float, int> temp_list = new SortedList<float, int>();
        //        for (int i = 0; i < m_context.m_waypoints.Length; i++)
        //        {
        //            temp_list.Add(Mathf.Pow(m_context.m_waypoints[i].transform.position.x - m_context.transform.position.x, 2) + Mathf.Pow(m_context.m_waypoints[i].transform.position.y - m_context.transform.position.y, 2), i);
        //        }
        //        m_context.m_index = temp_list.Values[0];
        //        m_next = m_if_reached_target;
        //        return false;
        //    }
        //}
        //else if(m_context.m_collide_with_player)
        //{
        //    m_next = m_if_reached_target;
        //    m_context.m_player.GetComponent<Player_Controller>().Respawn();
        //    return false;
        //}
        return true;
    }
}

