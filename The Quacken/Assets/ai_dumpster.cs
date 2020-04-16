using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ai_dumpster : MonoBehaviour
{
    public GameObject m_path;
    Rigidbody2D m_rb;
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    enum State
    {
        WAIT,
        ENTER_MOVE,
        MOVE
    }
    State m_current_state;
    int m_current = 0;
    void FixedUpdate()
    {
        switch (m_current_state)
        {
            case State.WAIT:
                m_current_state = State.ENTER_MOVE;
                break;

            case State.ENTER_MOVE:
                if (m_current >= m_path.transform.childCount)
                    m_current = 0;
                Start_Moving(m_path.transform.GetChild(m_current).transform.position);
                m_current++;
                m_current_state = State.MOVE;
                break;

            case State.MOVE:
                if (Moving())
                {
                    m_rb.velocity = Vector2.zero;
                    m_current_state = State.WAIT;
                }
                break;
        }
    }

    public float m_speed;
    Vector2 m_from;
    float m_to_move_distance;
    Vector2 m_move_direction;
    void Start_Moving(Vector2 p_target)
    {
        m_from = transform.position;
        m_move_direction = (p_target - (Vector2)transform.position).normalized;
        m_to_move_distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - p_target.x), 2) + Mathf.Pow((transform.position.y - p_target.y), 2));
    }

    bool Moving()
    {
        float moved_distance = Mathf.Sqrt(Mathf.Pow((m_from.x - transform.position.x), 2) + Mathf.Pow((m_from.y - transform.position.y), 2));

        m_rb.velocity = m_move_direction * m_speed;

        return moved_distance >= m_to_move_distance;
    }
}
