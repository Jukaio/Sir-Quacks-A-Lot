using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simulation of classic 8-Axis movement 
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2D_Movement : MonoBehaviour
{
    [SerializeField] private float m_speed = 5.0f;


    // Rigidbody Movement 
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
        
        m_rb.velocity = m_direction * m_speed;
        m_direction = Vector2.zero;
    }


    // Interpolated Movement
    Vector2 m_from;
    Vector2 m_target;
    float m_factor;
    public void Enter_Movement(Vector2 p_target)
    {
        m_from = transform.position;
        m_target = p_target;
        m_factor = 0.0f;
    }

    public bool Movement()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(m_from.x - m_target.x, 2) + Mathf.Pow(m_from.y - m_target.y, 2));
        m_factor += (m_speed / 10.0f) / distance;
        Interpolate(ref m_factor);

        m_direction = (m_target - m_from).normalized;
        if (direction != Vector2.zero)
            m_prev_direction = m_direction;
        transform.position = m_from + (m_target - m_from) * m_factor;

        return m_factor >= 1.0f;
    }


    static void Interpolate(ref float f)
    {
        if (f >= 1.0f)
            f = 1.0f;
        else if (f < 0.0f)
            f = 0.0f;
    }
}
