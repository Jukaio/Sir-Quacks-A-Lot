using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dumpster_Script : MonoBehaviour
{
    [SerializeField] Enemy_Data m_data;
    private GameObject m_player;


    public Animator m_anim;

    Physics2D_Movement m_movement;
    Seeing m_seeing;
    Hearing m_hearing;


    void Start()
    {
        m_seeing = GetComponent<Seeing>();
        m_hearing = GetComponent<Hearing>();
        m_player = Service<Game_Manager>.Get().Player;

        m_anim = GetComponent<Animator>();

        m_movement = GetComponent<Physics2D_Movement>();
        m_state = Movement_States.UP;
    }



    enum Movement_States
    {
        UP, DOWN, LEFT, RIGHT, CHASE
    }

    public UnityEngine.UI.Text text;
    Movement_States m_state;
    void Update()
    {
        m_movement.Reset_Direction();

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
                if (!m_seeing.See(m_movement.direction, m_player))
                    m_state = Movement_States.UP;
                break;
        }

        m_anim.SetFloat("x", m_movement.direction.x);
        m_anim.SetFloat("y", m_movement.direction.y);
        m_anim.SetFloat("prev_x", m_movement.prev_direction.x);
        m_anim.SetFloat("prev_y", m_movement.prev_direction.y);

        if (m_seeing.See(m_movement.direction, m_player))
            m_state = Movement_States.CHASE;
        if (m_hearing.Hear(m_player, m_player.GetComponent<Player_Controller>().m_noise_range))
            Debug.LogWarning("I hear");
    }

    void FixedUpdate()
    {
        m_movement.Execute();
    }
}
