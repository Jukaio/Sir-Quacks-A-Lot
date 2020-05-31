using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game_Input;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    public static int m_times_caught = 0;
    public static float m_time_passed = 0.0f;
    public bool m_is_game_play;

    public Text m_time_text;
    public Text m_death_text;
    public Canvas m_canvas;

    public float m_noise_range = 3.0f;
    public CompositeCollider2D m_objects_composite_collider;
    public Player_Input_System m_input;
    public Physics2D_Movement m_movement;
    public Animator m_anim;

    public GameObject m_shadow;

    public AudioClip m_get_caught_sound;
    AudioSource m_caught_source;

    public AudioClip m_movement_sound;
    AudioSource m_movement_source;

    public AudioClip m_reset_sound;
    AudioSource m_reset_source;

    public bool m_in_barrel = false;
    public bool m_is_looking_out_of_barrel = false;

    private void Awake()
    {
        m_movement_source = gameObject.AddComponent<AudioSource>();
        m_movement_source.clip = m_movement_sound;
        m_movement_source.volume = 0.75f;
        m_movement_source.loop = false;

        m_caught_source = gameObject.AddComponent<AudioSource>();
        m_caught_source.clip = m_get_caught_sound;
        m_caught_source.loop = false;

        m_reset_source = gameObject.AddComponent<AudioSource>();
        m_reset_source.clip = m_reset_sound;
        m_reset_source.loop = false;

        StartCoroutine(Play_Movement_Sound());

        m_anim = GetComponent<Animator>();
        m_movement = GetComponent<Physics2D_Movement>();

        Service<Game_Manager>.Get().Set_Player(gameObject);
    }

    void Start()
    {
        m_spawn_position = transform.position;
        if (m_is_game_play)
        {
            m_times_caught = 0;
            m_time_passed = 0.0f;
            m_death_text.text = "00" + m_times_caught.ToString();
        }

        m_input = Player_Input_System.Player(0);
    }

    void Update_Noise_Range()
    {
        transform.GetChild(0).transform.localScale = (Vector3.one / 10.0f) * m_noise_range;
    }

    void Go_In_Barrel()
    {
        if(m_shadow == null)
            m_shadow = GetComponent<Shadow_Renderer>().m_mesh_object;

        m_anim.SetTrigger("enter_barrel");
    }

    void Go_Out_Barrel()
    {
        m_shadow.SetActive(true);
        m_anim.SetTrigger("exit_barrel");
    }

    void Handle_Inputs()
    {
        m_movement.Reset_Direction();

        if (!death_running)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Go_In_Barrel();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Go_Out_Barrel();
            }

            if (!m_in_barrel)
            {
                if (m_input.Move_Left || Input.GetKey(KeyCode.LeftArrow))
                    m_movement.Add_Direction(Vector2.left);
                if (m_input.Move_Right || Input.GetKey(KeyCode.RightArrow))
                    m_movement.Add_Direction(Vector2.right);
                if (m_input.Move_Up || Input.GetKey(KeyCode.UpArrow))
                    m_movement.Add_Direction(Vector2.up);
                if (m_input.Move_Down || Input.GetKey(KeyCode.DownArrow))
                    m_movement.Add_Direction(Vector2.down);
            }
        }

        m_anim.SetFloat("x", m_movement.move_direction.x);
        m_anim.SetFloat("y", m_movement.move_direction.y);
        m_anim.SetFloat("prev_x", m_movement.prev_move_direction.x);
        m_anim.SetFloat("prev_y", m_movement.prev_move_direction.y);
    }

    public Vector2 m_spawn_position;
    bool death_running;

    IEnumerator Play_Death(GameObject obj)
    {
        if(m_in_barrel)
            Go_Out_Barrel();

        death_running = true;

        var ai = obj.GetComponent<Enemy_Base>();
        var other_rb = obj.GetComponent<Rigidbody2D>();
        var player_renderer = GetComponent<SpriteRenderer>();
        var collider = GetComponent<Collider2D>();


        collider.enabled = false;
        ai.active_ai = false;
        ai.m_seeing.gameObject.SetActive(false);
        player_renderer.enabled = false;
        other_rb.velocity = Vector2.zero;

        Vector3 enemy_reset_position = transform.position;

        Vector2 direction = obj.transform.position - transform.position;
        direction.Normalize();


        ai.m_anim.SetTrigger("catch");
        ai.m_anim.SetFloat("catch_x", direction.x);
        ai.m_anim.SetFloat("catch_y", direction.y);

        m_caught_source.Play();

        m_times_caught++;
        int temp = Mathf.RoundToInt(m_times_caught);
        string death_text;
        if (temp < 10)
            death_text = "00" + temp.ToString();
        else if (temp < 100)
            death_text = "0" + temp.ToString();
        else if (temp < 1000)
            death_text = temp.ToString();
        else
            death_text = "999";
        m_death_text.text = death_text;

        yield return new WaitForSeconds(1.5f);

        ai.m_anim.SetTrigger("drag");
        float distance = 0;
        Vector2 prev_position = obj.transform.position;

        while(distance < 12.0f)
        {
            Vector2 position = obj.transform.position;
            distance = prev_position.Distancef(position);

            obj.transform.position += (Vector3)direction * 2.5f * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }


        transform.position = m_spawn_position;
        obj.transform.position = enemy_reset_position;
        ai.m_anim.SetTrigger("reset");
        m_reset_source.Play();
        player_renderer.enabled = true;
        ai.active_ai = true;
        ai.m_seeing.gameObject.SetActive(true);
        death_running = false;
        collider.enabled = true;
        ai.fade_out();
        ai.m_seeing.fade_out();

    }

    public void Respawn(GameObject obj)
    {
        if (!death_running)
            StartCoroutine(Play_Death(obj));
    }

    void Execute_Inputs()
    {
        m_movement.Execute();

        if(m_in_barrel)
        {
            m_shadow.SetActive(m_is_looking_out_of_barrel);
        }
    }

    private void Update()
    {
        Handle_Inputs();
        Execute_Inputs();

        if (m_is_game_play)
        {
            m_time_passed += Time.deltaTime;
            int temp = Mathf.RoundToInt(m_time_passed);
            string time_text;
            if(temp < 10)
                time_text = "00" + temp.ToString();
            else if(temp < 100)
                time_text = "0" + temp.ToString();
            else if(temp < 1000)
                time_text = temp.ToString();
            else
                time_text = "999";

            m_time_text.text = time_text;
        }
    }

    void FixedUpdate()
    {
        Update_Noise_Range();

        var position = transform.position;

    }

    float m_distance_travelled = 0.0f;
    public float m_movement_treshhold = 1.0f;
    IEnumerator Play_Movement_Sound()
    {
        var previous_position = transform.position;

        while (true)
        {
            var position = transform.position;

            m_distance_travelled += position.Distancef(previous_position);

            if (m_distance_travelled > m_movement_treshhold)
            {
                m_movement_source.Play();
                m_distance_travelled = 0.0f;
            }

            previous_position = position;
            yield return new WaitForEndOfFrame();
        }
    }
}

