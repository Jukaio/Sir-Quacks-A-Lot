using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy_Base : MonoBehaviour
{
    private protected Animator m_anim;
    private protected Physics2D_Movement m_movement;
    private protected GameObject m_player;
    private protected Seeing m_seeing;
    private protected Hearing m_hearing;

    // Start is called before the first frame update
    void Start()
    {
        m_seeing = GetComponent<Seeing>();
        m_hearing = GetComponent<Hearing>();
        m_player = Service<Game_Manager>.Get().Player;
        m_anim = GetComponent<Animator>();
        m_movement = GetComponent<Physics2D_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        m_movement.Reset_Direction();

        Behaviour();
        Animate();
    }

    abstract public void Behaviour();
    abstract public void Animate();


    void FixedUpdate()
    {
        m_movement.Execute();
    }
}
