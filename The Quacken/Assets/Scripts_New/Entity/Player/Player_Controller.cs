using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game_Input;

public class Player_Controller : MonoBehaviour
{
    public float m_noise_range = 3.0f;
    public CompositeCollider2D m_objects_composite_collider;
    public Player_Input_System m_input;
    public Physics2D_Movement m_movement;
    public Animator m_anim;

    private void Awake()
    {

        m_anim = GetComponent<Animator>();
        m_movement = GetComponent<Physics2D_Movement>();
        Service<Game_Manager>.Get().Set_Player(gameObject);
    }

    void Start()
    {
        m_spawn_position = transform.position;
        m_input = Player_Input_System.Player(0);
    }

    void Update_Noise_Range()
    {
        transform.GetChild(0).transform.localScale = (Vector3.one / 10.0f) * m_noise_range;
    }

    void Handle_Inputs()
    {
        m_movement.Reset_Direction();
        if (m_input.Move_Left)
            m_movement.Add_Direction(Vector2.left);
        if (m_input.Move_Right)
            m_movement.Add_Direction(Vector2.right);
        if (m_input.Move_Up)
            m_movement.Add_Direction(Vector2.up);
        if (m_input.Move_Down)
            m_movement.Add_Direction(Vector2.down);

        m_anim.SetFloat("x", m_movement.move_direction.x);
        m_anim.SetFloat("y", m_movement.move_direction.y);
        m_anim.SetFloat("prev_x", m_movement.prev_move_direction.x);
        m_anim.SetFloat("prev_y", m_movement.prev_move_direction.y);
    }

    public Vector2 m_spawn_position;
    public void Respawn()
    {
        transform.position = m_spawn_position;
    }

    void Execute_Inputs()
    {
        m_movement.Execute();
    }

    private void Update()
    {
        Handle_Inputs();
        Execute_Inputs();
    }

    void FixedUpdate()
    {
        Update_Noise_Range();
    }

    private void OnGUI()
    {
        
    }
}

