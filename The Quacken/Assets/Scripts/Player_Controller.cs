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
    GameObject m_trail;
    public Material m_test_mat;
    // !Smell

    private void Awake()
    {
        m_game_objects = new GameObject[10];
        m_trail = new GameObject("Trail");
        for (int i = 0; i < 10; i++)
        {
            m_game_objects[i] = new GameObject("smell");
            Mesh mesh = new Mesh();
            Material material = new Material(m_test_mat);

            //m_game_objects[i].SetActive(false);
            //m_game_objects[i].AddComponent<CircleCollider2D>().isTrigger = true;
            //m_game_objects[i].layer = LayerMask.NameToLayer("Light_Overlay");
            //m_game_objects[i].AddComponent<MeshRenderer>().material = material;
            //m_game_objects[i].AddComponent<MeshFilter>().sharedMesh = mesh;

            //int[] triangles =  { 0, 1, 2, 0, 2, 3 };
            //m_game_objects[i].GetComponent<MeshFilter>().sharedMesh.triangles = triangles;
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

    GameObject m_prev_prev = null;
    GameObject m_prev = null;
    GameObject m_current;
    int m_smell_index = 0;
    void Execute_Inputs()
    {
        m_movement.Execute();
    }

    private void Update()
    {
        Handle_Inputs();

        if (m_smell_index < 10)
        {
            m_distance_walked -= Vector2.Distance(old_position, transform.position);
            if (m_distance_walked < 0)
            {
                m_current = m_game_objects[m_smell_index];
                m_current.SetActive(true);
                m_current.transform.parent = transform;
                m_current.transform.position = transform.position;

                if (m_prev != null)
                {
                    if (m_prev_prev != null)
                    {
                        m_prev.transform.parent = m_prev_prev.transform;
                    }
                    else
                        m_prev.transform.parent = m_trail.transform;
                }
                m_prev_prev = m_prev;
                m_prev = m_current;
                m_smell_index++;
                m_distance_walked = m_distance_interval;
            }
        }

        //for (int i = 0; i < 10; i++)
        //{
        //    m_game_objects[i].GetComponent<MeshFilter>().sharedMesh.Clear();

        //    int[] triangles = { 0, 1, 2, 0, 2, 3 };
        //    Vector3[] verts = {m_game_objects[i].transform.position - (Vector3.down * 5),
        //                       m_game_objects[i].transform.position - (Vector3.left * 5),
        //                       m_game_objects[i].transform.position - (Vector3.up * 5),
        //                       m_game_objects[i].transform.position - (Vector3.right * 5)};
        //    m_game_objects[i].GetComponent<MeshFilter>().sharedMesh.triangles = triangles;
        //    m_game_objects[i].GetComponent<MeshFilter>().sharedMesh.vertices = verts;
        //}



        for (int i = 0; i < 10; i++)
        {
            if (m_game_objects[i].transform.childCount != 0)
            {
                Debug.DrawLine(m_game_objects[i].transform.position, m_game_objects[i].transform.GetChild(0).transform.position, Color.red);
            }
        }
        old_position = transform.position;
    }

    void FixedUpdate()
    {
        Update_Noise_Range();
        Execute_Inputs();
    }
}

