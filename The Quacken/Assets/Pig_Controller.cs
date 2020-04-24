using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig_Controller : Enemy_Base
{
    enum Movement_States
    {
        UP, DOWN, LEFT, RIGHT, CHASE
    }
    Movement_States m_state;

    public override void Behaviour()
    {
        switch (m_state)
        {
            case Movement_States.UP:
                m_movement.Add_Direction(Vector2.up);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity).centroid) < 1.25f)
                    m_state = Movement_States.LEFT;
                break;
            case Movement_States.DOWN:
                m_movement.Add_Direction(Vector2.down);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity).centroid) < 1.25f)
                    m_state = Movement_States.RIGHT;
                break;
            case Movement_States.LEFT:
                m_movement.Add_Direction(Vector2.left);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity).centroid) < 1.5f)
                    m_state = Movement_States.DOWN;
                break;
            case Movement_States.RIGHT:
                m_movement.Add_Direction(Vector2.right);
                if (Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity).centroid) < 1.5f)
                    m_state = Movement_States.UP;
                break;

            case Movement_States.CHASE:
                m_movement.Add_Direction((m_player.transform.position - transform.position).normalized);
                if (!m_seeing.Sense(m_movement.direction, m_player))
                    m_state = Movement_States.UP;
                break;
        }


        if (m_seeing.Sense(m_movement.direction, m_player))
            m_state = Movement_States.CHASE;
        if (m_hearing.Sense(m_player.GetComponent<Player_Controller>().m_noise_range, m_player))
            Debug.LogWarning("I hear");
    }
}
