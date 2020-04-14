using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simulation of classic 8-Axis movement 
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2D_Movement : MonoBehaviour
{
    private Entity_Data m_data;

    private Vector2 m_direction;
    private Vector2 m_prev_direction;
    public Vector2 direction
    {
        get
        {
            return m_direction;
        }
        private set
        {
            m_direction = direction;
        }
    }
    public Vector2 prev_direction
    {
        get
        {
            return m_prev_direction;
        }
        private set
        {
            m_prev_direction = direction;
        }
    }
    private Rigidbody2D m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_direction = Vector2.zero;
    }

    public void Set_Data(Entity_Data p_data)
    {
        m_data = p_data;
    }

    public void Reset_Direction()
    {
        m_direction = Vector2.zero;
    }

    public void Add_Direction(Vector2 p_direction)
    {
        m_direction += p_direction;
    }

    public void Execute()
    {
        m_direction.Normalize();
        if (direction != Vector2.zero)
            m_prev_direction = m_direction;
        
        m_rb.velocity = m_direction * m_data.m_speed;
        m_direction = Vector2.zero;
    }
}
