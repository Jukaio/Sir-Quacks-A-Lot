using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy_Base : MonoBehaviour
{
    public Animator m_anim;
    public Physics2D_Movement m_movement;
    public GameObject m_player;
    public Seeing m_seeing;
    public Hearing m_hearing;
    public SpriteRenderer m_renderer;

    public bool m_sees;
    public bool m_collides_with_player;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();

        m_seeing = transform.parent.GetComponentInChildren<Seeing>();
        m_seeing.Set_Body(gameObject);
        
        m_hearing = GetComponent<Hearing>();
        m_player = Service<Game_Manager>.Get().Player;
        m_anim = GetComponent<Animator>();
        m_movement = GetComponent<Physics2D_Movement>();

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_sees = m_seeing.Sense(m_movement.view_direction, m_player);
        Behaviour();
    }

    abstract public void Init();
    abstract public void Behaviour();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "lightCollider":
                m_renderer.enabled = true;
                break;

            case "Player":
                m_collides_with_player = true;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "lightCollider":
                m_renderer.enabled = false;
                break;

            case "Player":
                m_collides_with_player = false;
                break;
        }
    }
}
