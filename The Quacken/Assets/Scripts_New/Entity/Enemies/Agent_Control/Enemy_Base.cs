using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy_Base : MonoBehaviour
{
    public Animator m_anim;
    public Physics2D_Movement m_movement;
    public GameObject m_player;
    private Player_Controller m_player_controller;
    public Seeing m_seeing;
    public Hearing m_hearing;
    public SpriteRenderer m_renderer;

    public bool m_sees;
    public bool m_prev_sees;

    public bool m_remembers;

    public AudioClip m_notice_clip;
    public AudioSource m_notice_source;

    public float pitch = 0.65f;

    // Start is called before the first frame update
    void Start()
    {
        m_notice_source = gameObject.AddComponent<AudioSource>();
        m_notice_source.clip = m_notice_clip;
        m_notice_source.loop = false;
        m_notice_source.pitch = pitch;
        m_notice_source.playOnAwake = false;

        m_renderer = GetComponent<SpriteRenderer>();

        m_seeing = transform.parent.GetComponentInChildren<Seeing>();
        m_seeing.Set_Body(gameObject);
        
        m_hearing = GetComponent<Hearing>();
        m_player = Service<Game_Manager>.Get().Player;
        m_player_controller = m_player.GetComponent<Player_Controller>();
        m_anim = GetComponent<Animator>();
        m_movement = GetComponent<Physics2D_Movement>();

        m_renderer.color = new Color(1, 1, 1, 0);

        Init();
    }

    bool fade_in_running;
    bool fade_out_running;

    public void Play_Notice()
    {
        m_notice_source.Play();
    }

    public bool active_ai = true;

    // Update is called once per frame
    void Update()
    {
        if (active_ai)
        {
            m_sees = m_seeing.Sense(m_movement.view_direction, m_player) && !m_player_controller.m_in_barrel;
            if(m_prev_sees && m_player_controller.m_in_barrel)
                m_sees = true;

            Behaviour();


            m_prev_sees = m_sees;
        }

    }

    abstract public void Init();
    abstract public void Behaviour();

    public void fade_out()
    {
        StartCoroutine(fade_out_color());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "lightCollider":
                StartCoroutine(fade_in_color());
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "lightCollider":
                if (!fade_out_running && !fade_in_running && m_renderer.color.a != 1)
                    StartCoroutine(fade_in_color());
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "lightCollider":
                StartCoroutine(fade_out_color());
                break;
        }
    }

    IEnumerator fade_out_color()
    {
        while (m_renderer.color.a > 0 && !fade_in_running)
        {
            fade_out_running = true;
            var factor = m_renderer.color.a;
            Utility.Extra_Math.Interpolate(ref factor);

            m_renderer.color -= (Color.black * Time.deltaTime * 8.0f);
            yield return new WaitForEndOfFrame();
        }

        fade_out_running = false;
        if (m_renderer.color.a != 0)
            m_renderer.color = new Color(1, 1, 1, 0);
    }

    IEnumerator fade_in_color()
    {
        while (m_renderer.color.a < 1 && !fade_out_running)
        {
            fade_in_running = true;
            var factor = m_renderer.color.a;
            Utility.Extra_Math.Interpolate(ref factor);

            m_renderer.color += (Color.black * Time.deltaTime * 8.0f);
            yield return new WaitForEndOfFrame();
        }

        fade_in_running = false;
        if (m_renderer.color.a != 1)
            m_renderer.color = new Color(1, 1, 1, 1);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                m_player.GetComponent<Player_Controller>().Respawn(gameObject);
                break;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
