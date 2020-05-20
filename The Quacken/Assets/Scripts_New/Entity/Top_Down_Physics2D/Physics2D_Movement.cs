using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simulation of classic 8-Axis movement 
[RequireComponent(typeof(Rigidbody2D))]
public class Physics2D_Movement : MonoBehaviour
{
    [SerializeField] private float m_rotate_speed = 5.0f;
    public float Rotate_Speed
    {
        get => m_rotate_speed;
    }

    private float m_init_speed; // initial speed
    public float Initial_Speed
    {
        get => m_init_speed;
    }

    [SerializeField] private float m_speed = 5.0f;
    public float Speed { get => m_speed; }


    // Rigidbody Movement 
    private Vector2 m_move_direction;
    private Vector2 m_prev_move_direction;
    private Vector2 m_view_direction;
    private Vector2 m_prev_view_direction;

    public Vector2 move_direction
    {
        get
        {
            return m_move_direction;
        }
        private set
        {
            m_move_direction = move_direction;
        }
    }
    public Vector2 prev_move_direction
    {
        get
        {
            return m_prev_move_direction;
        }
        private set
        {
            m_prev_move_direction = move_direction;
        }
    }
    public Vector2 view_direction
    {
        get => m_view_direction;
        set => m_view_direction = value;
    }
    public Vector2 prev_view_direction
    {
        get => m_prev_view_direction;
        set => m_prev_view_direction = value;
    }

    private Rigidbody2D m_rb;
    
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_move_direction = Vector2.zero;
        m_view_direction = Vector2.zero;
        m_init_speed = m_speed;
    }

    public void Set_Speed(float p_speed)
    {
        m_speed = p_speed;
    }

    public void Reset_Direction()
    {
        m_move_direction = Vector2.zero;
    }

    public void Add_Direction(Vector2 p_direction)
    {
        m_move_direction += p_direction;
        m_view_direction = m_move_direction;
    }

    public void Normalise()
    {
        m_move_direction.Normalize();
        m_prev_move_direction.Normalize();
    }

    public void Execute()
    {
        m_move_direction.Normalize();
        if (move_direction != Vector2.zero)
            m_prev_move_direction = m_move_direction;
        
        m_rb.velocity = m_move_direction * m_speed;
        m_move_direction = Vector2.zero;
    }




    // Interpolated Movement
    Vector2 m_move_from;
    Vector2 m_move_target;
    float m_move_factor;
    public void Enter_Move(Vector2 p_target)
    {
        m_move_from = transform.position;
        m_move_target = p_target;
        m_move_factor = 0.0f;
    }

    public bool Move()
    {
        float distance = Mathf.Sqrt(Mathf.Pow(m_move_from.x - m_move_target.x, 2) + Mathf.Pow(m_move_from.y - m_move_target.y, 2));
        m_move_factor += (m_speed / distance) * Time.deltaTime;
        Utility.Extra_Math.Interpolate(ref m_move_factor);

        m_move_direction = (m_move_target - m_move_from).normalized;
        if (move_direction != Vector2.zero)
            m_prev_move_direction = m_move_direction;
        m_rb.position = m_move_from + (m_move_target - m_move_from) * m_move_factor;

        m_view_direction = m_move_direction;

        return m_move_factor >= 1.0f;
    }

    // Interpolated Rotation
    Vector2 m_rotate_from;
    Vector2 m_rotate_target;
    float m_rotate_angle;
    float m_rotate_factor;
    public void Enter_Rotation(float p_target)
    {
        m_rotate_from = m_view_direction;
        m_rotate_target = m_view_direction.Rotate(p_target);
        m_rotate_angle = p_target;
        m_rotate_factor = 0.0f;
    }

    public bool Rotate()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3)view_direction * 3.0f, Color.red);
        m_rotate_factor += Mathf.Abs((m_rotate_speed / m_rotate_angle) * Time.deltaTime);
        Utility.Extra_Math.Interpolate(ref m_rotate_factor);
        m_view_direction = m_rotate_from.Rotate(m_rotate_angle * m_rotate_factor);

        return m_rotate_factor >= 1.0f;
    }
}
