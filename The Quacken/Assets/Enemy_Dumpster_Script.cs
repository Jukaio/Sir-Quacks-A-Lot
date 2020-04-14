using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dumpster_Script : MonoBehaviour
{
    ///////////////////////
    /// SerializeFields ///
    ///////////////////////

    // Seeing
    [SerializeField] private float m_cone_width = 30.0f;
    [SerializeField] private float m_cone_length = 5.0f;
    [SerializeField] private string m_tag_to_compare = "Player";
    // Hearing
    [SerializeField] private float m_hearing_range = 3.0f;

    ////////////////////////
    /// !SerializeFields ///
    ////////////////////////

    // to_player
    // Seeing
    // Cone
    private GameObject m_player;
    Vector2 m_to_player_direction;
    float m_to_player_distance;
    float m_to_player_angle;
    // Raycast - If player is behind a collider or not
    RaycastHit2D m_hit = new RaycastHit2D();

    // hearing
    float m_player_noise_range;
    // !to_player

    // this (Enemy)
    Vector2 m_origin = Vector2.up;
    Vector2 m_view_direction;
    float m_view_angle;
    // !this

    // Interface Visuals for early development
    bool sees; // For debugging
    bool hears; // For debugging

    public Animator m_anim;

    Physics2D_Movement m_movement;
    public Entity_Data m_data;
    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_player = Service<Game_Manager>.Get().Player;
        m_player_noise_range = m_player.GetComponent<Player_Controller>().m_noise_range;
        Update_Hearing_Range();

        m_movement = GetComponent<Physics2D_Movement>();
        m_movement.Set_Data(m_data);
        m_state = Movement_States.UP;
    }

    void Update_Hearing_Range()
    {
        transform.GetChild(0).transform.localScale = (Vector3.one / 10.0f) * m_hearing_range;
    }

    enum Movement_States
    {
        UP, DOWN, LEFT, RIGHT
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
        }

        m_anim.SetFloat("x", m_movement.direction.x);
        m_anim.SetFloat("y", m_movement.direction.y);
        m_anim.SetFloat("prev_x", m_movement.prev_direction.x);
        m_anim.SetFloat("prev_y", m_movement.prev_direction.y);

        sees = Sees_Player();
        hears = Hears_Player();

        Color color = Color.white;
        if(hears)
        {
            color = Color.red;
        }

        if(hears || sees)
        {
            
        }
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

    }

    void FixedUpdate()
    {
        m_movement.Execute();
    }

    void Set_Distance_To_Player()
    {
        m_to_player_direction = (m_player.transform.position - transform.position).normalized;
        m_to_player_distance = Vector2.Distance(transform.position, m_player.transform.position);
    }

    bool Player_In_Vision_Range()
    {
        m_view_direction = m_movement.direction.Rotate(transform.rotation.eulerAngles.z);
        m_view_angle = Vector2.Angle(m_movement.direction, m_view_direction);
        m_to_player_angle = Vector2.Angle(m_movement.direction, m_to_player_direction.normalized);

        return  m_to_player_angle > m_view_angle - m_cone_width && m_to_player_angle < m_view_angle + m_cone_width &&
                m_to_player_distance < m_cone_length;
    }

    bool Sees_Player()
    {
        Set_Distance_To_Player();
        if (Player_In_Vision_Range())
        {
            m_hit = Physics2D.Raycast(transform.position, m_to_player_direction * m_to_player_distance);
            Debug.DrawRay(transform.position, m_to_player_direction * m_to_player_distance, Color.green);
            if (m_hit.collider.CompareTag(m_tag_to_compare))
                return true;
        }
        return false;
    }

    bool Hears_Player()
    {
        return m_to_player_distance < m_player_noise_range + m_hearing_range;
    }

}
