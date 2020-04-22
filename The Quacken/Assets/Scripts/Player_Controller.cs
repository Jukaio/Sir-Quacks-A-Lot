using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

public class Player_Controller : MonoBehaviour
{

    public float m_noise_range = 3.0f;
    public CompositeCollider2D m_objects_composite_collider;
    public Player_Input m_input;
    public Physics2D_Movement m_movement;
    public Player_Data m_data;
    public Animator m_anim;

    //Smell
    float m_distance_interval = 5.0f;
    float m_distance_walked;
    Vector2 old_position;
    public GameObject m_template_smell;
    GameObject[] m_game_objects;
    // !Smell

    private void Awake()
    {
        m_game_objects = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            m_game_objects[i] = new GameObject("smell");
            m_game_objects[i].SetActive(false);
            m_game_objects[i].AddComponent<CircleCollider2D>().isTrigger = true;
        }

        m_anim = GetComponent<Animator>();
        m_movement = GetComponent<Physics2D_Movement>();
        Service<Game_Manager>.Get().Set_Player(gameObject);
    }

    void Start()
    {
        m_input = Player_Input.Player(0);
        m_distance_walked = m_distance_interval;
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

        m_anim.SetFloat("x", m_movement.direction.x);
        m_anim.SetFloat("y", m_movement.direction.y);
        m_anim.SetFloat("prev_x", m_movement.prev_direction.x);
        m_anim.SetFloat("prev_y", m_movement.prev_direction.y);
    }

    GameObject m_prev = null;
    int m_smell_index = 0;
    void Execute_Inputs()
    {
        m_movement.Execute();

        if (m_smell_index < 10)
        {
            m_distance_walked -= Vector2.Distance(old_position, transform.position);
            if (m_distance_walked < 0)
            {
                m_game_objects[m_smell_index].transform.position = transform.position;
                m_game_objects[m_smell_index].SetActive(true);
                if (m_prev != null)
                    m_game_objects[m_smell_index].transform.parent = m_prev.transform;

                m_prev = m_game_objects[m_smell_index];
                m_smell_index++;
                m_distance_walked = m_distance_interval;
            }
        }

        for(int i = 0; i < 10; i++)
        {
            if(m_game_objects[i].transform.childCount != 0)
            {
                Debug.DrawLine(m_game_objects[i].transform.position, m_game_objects[i].transform.GetChild(0).transform.position, Color.red);
            }
        }

        old_position = transform.position;
    }

    private void Update()
    {
        Handle_Inputs();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Service<Sound_Manager>.Get().Play("Music", "Test");
    }

    void FixedUpdate()
    {
        Update_Noise_Range();
        Execute_Inputs();
    }
}

